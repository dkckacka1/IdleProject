using System.Collections.Generic;
using Engine.Util.Extension;

namespace IdleProject.Battle.Character.Behaviour
{
    public class CharacterBehaviour 
    {
        private readonly IEnumerator<ExecuteBehaviour> _skillActionList;

        public CharacterBehaviour(List<ExecuteBehaviour> skillActionList)
        {
            _skillActionList = skillActionList.ListLoop().GetEnumerator();
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
    }
}