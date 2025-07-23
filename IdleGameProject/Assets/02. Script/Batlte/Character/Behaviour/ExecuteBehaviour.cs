using System.Collections.Generic;
using IdleProject.Battle.Character.Behaviour.SkillAction;

namespace IdleProject.Battle.Character.Behaviour
{
    public class ExecuteBehaviour
    {
        private readonly List<IBehaviourAction> _behaviourActions;
        private readonly bool _isSkillBehaviour;
        
        public ExecuteBehaviour(List<IBehaviourAction> behaviourActions, bool isSkillBehaviour)
        {
            _behaviourActions = behaviourActions;
            _isSkillBehaviour = isSkillBehaviour;
        }

        public void Execute()
        {
            foreach (var action in _behaviourActions)
            {
                action.ActionExecute(_isSkillBehaviour);
            }
        }
    }
}