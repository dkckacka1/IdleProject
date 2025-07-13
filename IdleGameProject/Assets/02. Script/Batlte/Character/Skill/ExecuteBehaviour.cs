using System.Collections.Generic;
using IdleProject.Battle.Character.Skill.SkillAction;

namespace IdleProject.Battle.Character.Skill
{
    public class ExecuteBehaviour
    {
        private readonly List<IBehaviourAction> _skillActionList;
        private readonly bool _isSkillBehaviour;
        
        public ExecuteBehaviour(List<IBehaviourAction> skillActionList, bool isSkillBehaviour)
        {
            _skillActionList = skillActionList;
            _isSkillBehaviour = isSkillBehaviour;
        }

        public void Execute()
        {
            foreach (var action in _skillActionList)
            {
                action.ActionExecute(_isSkillBehaviour);
            }
        }
    }
}