using System.Collections.Generic;
using Engine.Util.Extension;
using IdleProject.Battle.Character.Skill.SkillAction;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill
{
    public class CharacterSkill 
    {
        private readonly IEnumerator<ExecuteSkill> _skillActionList;

        public CharacterSkill(List<ExecuteSkill> skillActionList)
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