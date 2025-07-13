using IdleProject.Core;
using IdleProject.Data.SkillData;
using Sirenix.OdinInspector.Editor.TypeSearch;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill.SkillAction.Implement
{
    public class AttackAction : BehaviourAction
    {
        private bool _canCritical;
        private readonly float _attackValue;

        private readonly EffectAction _hitEffect;
        
        public AttackAction(AttackSkillActionData skillActionData, CharacterController controller) : base(skillActionData, controller)
        {
            _canCritical = skillActionData.canCritical;
            _attackValue = skillActionData.attackValue;

            if (skillActionData.hitEffect is not null)
            {
                _hitEffect = new EffectAction(skillActionData.hitEffect, controller);
            }
        }

        public override void ActionExecute()
        {
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * _attackValue;
            
            foreach (var target in GetTargetList.Invoke())
            {
                Controller.Hit(target, attackDamage);

                if (_hitEffect is not null)
                {
                    _hitEffect.SetTarget(target);
                    _hitEffect.ActionExecute();
                }
            }
        }
    }
}