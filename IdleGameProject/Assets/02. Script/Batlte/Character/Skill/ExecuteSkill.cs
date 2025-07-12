using System.Collections.Generic;
using IdleProject.Battle.Character.Skill.SkillAction;

namespace IdleProject.Battle.Character.Skill
{
    public class ExecuteSkill
    {
        private readonly List<ISkillAction> _skillActionList;
        
        public ExecuteSkill(List<ISkillAction> skillActionList)
        {
            _skillActionList = skillActionList;
        }

        public void Execute()
        {
            foreach (var action in _skillActionList)
            {
                action.ActionExecute();
            }
        }
    }
}