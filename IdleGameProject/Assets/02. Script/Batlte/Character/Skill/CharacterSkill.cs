using IdleProject.Battle.Effect;
using IdleProject.Battle.Projectile;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;

namespace IdleProject.Battle.Character.Skill
{
    public abstract class CharacterSkill
    {
        public CharacterController Controller;

        public void SetAnimationEvent(AnimationEventHandler eventHandler)
        {
            eventHandler.SkillActionEvent += SkillAction;
            eventHandler.SkillEffectEvent += SkillEffect;
        }
        
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

        private void SkillEffect(string effectParameter)
        {
            var split = effectParameter.Split(',');

            var effectName = split[0];
            var effectOffsetTransform = Controller.offset.GetOffsetTransform((CharacterOffsetType)int.Parse(split[1]));

            var effect = ObjectPoolManager.Instance.Get<BattleEffect>(effectName);
            effect.transform.position = effectOffsetTransform.position;
            effect.transform.rotation = effectOffsetTransform.rotation;
            effect.SetSkillEffect();
        }

        protected abstract void SkillAction(int skillNumber);
    }
}