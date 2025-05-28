using System;
using UnityEngine;

namespace IdleProject.Battle.Character.Skill
{
    public class CharacterSkillHiro : CharacterSkill
    {
        public override void SetAnimationEvent(AnimationEventHandler eventHandler)
        {
            eventHandler.SkillFirstEvent += SkillFirstHit;
        }

        private void SkillFirstHit()
        {
            var target = controller.GetTargetCharacter.Invoke();
            var attackDamage = controller.statSystem.GetStatValue(CharacterStatType.AttackDamage) * 1.6f;

            controller.Hit(target, attackDamage);

            var skillEffect = controller.GetSkillHitEffect.Invoke();
            skillEffect.transform.position = target.HitEffectOffset;
        }
    }
}