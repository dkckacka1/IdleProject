using IdleProject.Core;
using IdleProject.Data.BehaviourData;

namespace IdleProject.Battle.Character.Behaviour.SkillAction.Implement
{
    public class AttackAction : BehaviourAction
    {
        private readonly bool _canCritical;
        private readonly float _attackValue;

        private readonly EffectAction _hitEffect;
        
        public AttackAction(AttackBehaviourActionData behaviourActionData, CharacterController controller) : base(behaviourActionData, controller)
        {
            _canCritical = behaviourActionData.canCritical;
            _attackValue = behaviourActionData.attackValue;

            if (behaviourActionData.hitEffect is not null)
            {
                _hitEffect = new EffectAction(behaviourActionData.hitEffect, controller);
            }
        }

        public override void ActionExecute(bool isSkillBehaviour)
        {
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * _attackValue;
            
            foreach (var target in GetTargetList.Invoke())
            {
                Controller.Hit(target, attackDamage, _canCritical);

                if (_hitEffect is not null)
                {
                    _hitEffect.SetTarget(target);
                    _hitEffect.ActionExecute(isSkillBehaviour);
                }
            }
        }
    }
}