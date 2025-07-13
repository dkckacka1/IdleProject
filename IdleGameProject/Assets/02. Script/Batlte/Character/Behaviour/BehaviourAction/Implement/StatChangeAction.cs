using IdleProject.Core;
using IdleProject.Data.BehaviourData;

namespace IdleProject.Battle.Character.Behaviour.SkillAction.Implement
{
    public class StatChangeAction : BehaviourAction
    {
        private readonly bool _isAbsolute;
        private readonly CharacterStatType _statType;
        private readonly float _value;


        public StatChangeAction(StatChangeActionData actionData, CharacterController controller) : base(actionData, controller)
        {
            _isAbsolute = actionData.isAbsolute;
            _statType = actionData.statType;
            _value = actionData.value;
        }

        public override void ActionExecute(bool isSkillBehaviour)
        {
            foreach (var target in GetTargetList.Invoke())
            {
                if (_isAbsolute)
                {
                    target.StatSystem.SetStatValue(_statType, _value);
                }
                else
                {
                    target.StatSystem.SetStatValue(_statType, target.StatSystem.GetStatValue(_statType) + _value);
                }
            }
        }
    }
}