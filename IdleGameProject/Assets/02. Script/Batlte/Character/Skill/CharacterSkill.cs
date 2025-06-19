using IdleProject.Battle.Effect;
using IdleProject.Battle.Projectile;
using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill
{
    public abstract class CharacterSkill
    {
        public CharacterController Controller;

        public abstract void SetAnimationEvent(AnimationEventHandler eventHandler);

        protected bool TryGetSkillProjectile(out BattleProjectile projectile)
        {
            projectile = Controller.GetSkillProjectile?.Invoke();
            if (projectile)
            {
                projectile.SetSkillProjectile();
                return true;
            }
            
            return false;
        }

        protected bool TryGetSkillEffect(out BattleEffect effect)
        {
            effect = Controller.GetSkillHitEffect?.Invoke();
            if (effect)
            {
                effect.SetSkillEffect();
                return true;
            }
            
            return false;
        }
    }
}