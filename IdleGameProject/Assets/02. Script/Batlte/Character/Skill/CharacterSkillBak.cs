using IdleProject.Core;

namespace IdleProject.Battle.Character.Skill
{
    public sealed class CharacterSkillBak : CharacterSkill
    {
        protected override void SkillAction(int skillNumber)
        {
            if (TryGetSkillEffect(out var effect))
            {
                effect.effectTriggerEnterEvent.AddListener(HitTarget);
                effect.transform.position = Controller.HitEffectOffset;
            }
        }

        private void HitTarget(ITakeDamagedAble takeDamage)
        {
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * 0.6f;
            if (takeDamage.GetAiType != Controller.GetAiType)
            {
                Controller.Hit(takeDamage, attackDamage);
            }
        }
    }
}