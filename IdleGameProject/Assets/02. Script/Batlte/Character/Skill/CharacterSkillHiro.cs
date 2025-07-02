using System;
using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill
{
    public class CharacterSkillHiro : CharacterSkill
    {
        public override void SetAnimationEvent(AnimationEventHandler eventHandler)
        {
            eventHandler.SkillActionEvent += SkillAction;
        }

        private void SkillAction(int skillNumber)
        {
            var target = Controller.GetTargetCharacter.Invoke();
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * 1.6f;

            Controller.Hit(target, attackDamage);

            if (TryGetSkillEffect(out var effect))
            {
                effect.transform.position = target.HitEffectOffset;
            }
        }
    }
}