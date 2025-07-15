using System.Collections.Generic;
using System.Linq;
using Engine.Util.Extension;
using IdleProject.Battle.Character.Behaviour.Targeting;
using IdleProject.Core;

namespace IdleProject.Battle.Character.Behaviour
{
    public class CharacterBehaviour
    {
        private readonly IEnumerator<ExecuteBehaviour> _skillActionList;
        private readonly IEnumerable<IBehaviourTargeting> _behaviourTargetingList;

        private readonly CharacterController _useCharacter;

        public CharacterBehaviour(CharacterController useCharacter, List<ExecuteBehaviour> skillActionList,
            IEnumerable<IBehaviourTargeting> behaviourTargetingList)
        {
            _skillActionList = skillActionList.ListLoop().GetEnumerator();
            _behaviourTargetingList = behaviourTargetingList;
            _useCharacter = useCharacter;
        }

        // 스킬을 사용한다.
        public void ExecuteSkill()
        {
            // 무한 순회
            if (_skillActionList is not null && _skillActionList.MoveNext())
            {
                var currentSkill = _skillActionList.Current;

                // 각 액션을 실행
                currentSkill.Execute();
            }
        }

        public List<CharacterController> GetDefaultTargetList()
        {
            var targetList = new List<CharacterController>();
            var allCharacter = GameManager.GetCurrentSceneManager<BattleManager>().GetCharacterList();

            if (_behaviourTargetingList != null)
                targetList = _behaviourTargetingList.Aggregate(targetList,
                    (current, targeting) => targeting.GetTargetingCharacterList(_useCharacter, current, allCharacter));

            return targetList;
        }
    }
}