using IdleProject.Character.Character;
using IdleProject.Character.Stat;

namespace IdleProject.Character.Skill
{
    public class CharacterSkillBak : CharacterSkill
    {
        public override void SetAnimationEvent(AnimationEventHandler eventHandler)
        {
            eventHandler.SkillFirstEvent += SkillHit;
            eventHandler.SkillSecondEvent += SkillHit;
            eventHandler.SkillThirdEvent += SkillHit;
            eventHandler.SkillFourthEvent += SkillHit;
        }
        
        private void SkillHit()
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