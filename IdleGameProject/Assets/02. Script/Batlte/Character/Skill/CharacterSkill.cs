using System.Collections.Generic;
using IdleProject.Battle.Character.Skill.SkillAction;

namespace IdleProject.Battle.Character.Skill
{
    public class CharacterSkill
    {
        private readonly CharacterController _controller;

        private readonly List<ISkillAction> _skillActions;

        public CharacterSkill(List<SkillAction.SkillAction> skillActions)
        {
            _skillActions = new List<ISkillAction>(skillActions);
        }

        // 스킬을 사용한다.
        public void ExecuteSkill()
        {
            // 각 액션을 실행
            foreach (var action in _skillActions)
            {
                action.ActionExecute();
            }
        }
    }
}