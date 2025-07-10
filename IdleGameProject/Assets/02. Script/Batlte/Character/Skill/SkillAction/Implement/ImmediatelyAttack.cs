using System;
using System.Collections.Generic;
using IdleProject.Battle.Effect;
using IdleProject.Core;

namespace IdleProject.Battle.Character.Skill.SkillAction.Implement
{
    public class ImmediatelyAttack : ISkillAction
    {
        private readonly Func<BattleEffect> _getHitEffect;
        private readonly IEnumerator<float> _skillValues;

        public ImmediatelyAttack(Func<BattleEffect> getHitEffect, IEnumerable<float> skillValues)
        {
            _getHitEffect = getHitEffect;
            _skillValues = skillValues.GetEnumerator();
            _skillValues.MoveNext();
        }
        
        public void ExecuteSkillAction(CharacterController controller, IEnumerable<CharacterController> targetList)
        {
            var attackDamage = controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) *
                               _skillValues.Current;
            
            foreach (var target in targetList)
            {
                controller.Hit(target, attackDamage);
                
                var attackHitEffect = _getHitEffect?.Invoke();
                if (attackHitEffect)
                {
                    attackHitEffect.transform.position = target.HitEffectOffset;
                    attackHitEffect.transform.rotation = controller.transform.rotation;
                }
            }

            _skillValues.MoveNext();
        }
    }
}