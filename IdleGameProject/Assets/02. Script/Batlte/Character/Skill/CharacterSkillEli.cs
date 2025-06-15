using System;
using IdleProject.Battle.Projectile;
using IdleProject.Core;
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
            var target = Controller.GetTargetCharacter.Invoke();
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * 2f;

            if (TryGetSkillProjectile(out var projectile))
            {
                projectile.transform.position = Controller.offset.GetProjectilePosition;
                projectile.Target = target;
                projectile.SetSkillProjectile();
                projectile.hitEvent.AddListener(takeDamagedAble =>
                {
                    Controller.Hit(takeDamagedAble, attackDamage);

                    if (TryGetSkillEffect(out var effect))
                    {
                        effect.SetSkillEffect();
                        effect.transform.position = takeDamagedAble.HitEffectOffset;
                    }
                });
            }
        }
    }
}