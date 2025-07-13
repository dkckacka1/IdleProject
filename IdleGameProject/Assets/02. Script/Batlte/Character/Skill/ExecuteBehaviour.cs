using System.Collections.Generic;
using IdleProject.Battle.Character.Skill.SkillAction;

namespace IdleProject.Battle.Character.Skill
{
    public class ExecuteBehaviour
    {
        private readonly List<IBehaviourAction> _skillActionList;
        
        public ExecuteBehaviour(List<IBehaviourAction> skillActionList)
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