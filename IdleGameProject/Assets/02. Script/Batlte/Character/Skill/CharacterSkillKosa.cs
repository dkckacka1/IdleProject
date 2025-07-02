using IdleProject.Core;

namespace IdleProject.Battle.Character.Skill
{
    public class CharacterSkillKosa : CharacterSkill
    {
        protected override void SkillAction(int skillNumber)
        {
            var target = Controller.GetTargetCharacter.Invoke();
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * 1f;

            if (TryGetSkillProjectile(out var projectile))
            {
                projectile.transform.position =
                    Controller.offset.GetOffsetTransform(CharacterOffsetType.ProjectileOffset).position;
                projectile.Target = target;
                projectile.SetSkillProjectile();
                projectile.hitEvent.AddListener(takeDamagedAble =>
                {
                    Controller.Hit(takeDamagedAble, attackDamage);
                    if (TryGetSkillEffect(out var effect))
                    {
                        effect.SetSkillEffect();
                        effect.effectTriggerEnterEvent.AddListener(HitTarget);
                        effect.transform.position = takeDamagedAble.HitEffectOffset;
                    }
                });
            }
        }
        
        private void HitTarget(ITakeDamagedAble takeDamage)
        {
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * 3f;
            if (takeDamage.GetAiType != Controller.GetAiType)
            {
                Controller.Hit(takeDamage, attackDamage);
            }
        }
    }
}