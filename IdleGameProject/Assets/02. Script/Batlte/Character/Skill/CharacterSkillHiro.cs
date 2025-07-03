using IdleProject.Core;

namespace IdleProject.Battle.Character.Skill
{
    public class CharacterSkillHiro : CharacterSkill
    {
        protected override  void SkillAction(int skillNumber)
        {
            var target = Controller.GetTargetCharacter.Invoke();
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * 1.6f;

            Controller.Hit(target, attackDamage);

            if (TryGetSkillEffect(out var effect))
            {
                effect.transform.position = target.HitEffectOffset;
                effect.transform.rotation = Controller.transform.rotation;
            }
        }
    }
}