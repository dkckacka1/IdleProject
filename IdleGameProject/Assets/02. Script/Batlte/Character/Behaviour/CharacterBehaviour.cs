using System.Collections.Generic;
using System.Linq;
using Engine.Util.Extension;
using IdleProject.Battle.Character.Behaviour.Targeting;
using IdleProject.Core;

namespace IdleProject.Battle.Character.Behaviour
{
    public class CharacterBehaviour
    {
        private readonly IEnumerator<ExecuteBehaviour> _behaviourActionList;
        private readonly IEnumerable<IBehaviourTargeting> _behaviourTargetingList;

        private readonly CharacterController _useCharacter;

        public CharacterBehaviour(CharacterController useCharacter, List<ExecuteBehaviour> behaviourActionList,
            IEnumerable<IBehaviourTargeting> behaviourTargetingList)
        {
            _behaviourActionList = behaviourActionList.ListLoop().GetEnumerator();
            _behaviourTargetingList = behaviourTargetingList;
            _useCharacter = useCharacter;
        }

        // 스킬을 사용한다.
        public void ExecuteBehaviour()
        {
            // 무한 순회
            if (_behaviourActionList is not null && _behaviourActionList.MoveNext())
            {
                var currentBehaviour = _behaviourActionList.Current;

                // 각 액션을 실행
                currentBehaviour.Execute();
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