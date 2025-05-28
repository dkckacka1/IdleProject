using System;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill
{
    public class CharacterSkillEli : CharacterSkill
    {
        public override void SetAnimationEvent(AnimationEventHandler eventHandler)
        {
            eventHandler.SkillFirstEvent += SkillFirstHit;
        }

        private void SkillFirstHit()
        {
            var target = controller.GetTargetCharacter.Invoke();
            var attackDamage = controller.statSystem.GetStatValue(CharacterStatType.AttackDamage) * 2f;
            var skillProjectile = controller.GetSkillProjectile?.Invoke();

            skillProjectile.transform.position = controller.offset.CreateProjectileOffset;
            skillProjectile.target = target;
            skillProjectile.hitEvent.AddListener(target =>
            {
                controller.Hit(target, attackDamage);
                var attackHitEffect = controller.GetAttackHitEffect?.Invoke();
                attackHitEffect.transform.position = target.HitEffectOffset;
            });
        }
    }
}