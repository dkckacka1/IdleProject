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
            var target = Controller.GetTargetCharacter.Invoke();
            var attackDamage = Controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * 2f;
            var skillProjectile = Controller.GetSkillProjectile?.Invoke();

            if (skillProjectile)
            {
                skillProjectile.transform.position = Controller.offset.CreateProjectileOffset;
                skillProjectile.Target = target;
                skillProjectile.hitEvent.AddListener(takeDamagedAble =>
                {
                    Controller.Hit(takeDamagedAble, attackDamage);
                    
                    var attackHitEffect = Controller.GetAttackHitEffect?.Invoke();
                    if (attackHitEffect) 
                        attackHitEffect.transform.position = takeDamagedAble.HitEffectOffset;
                    
                    BattleManager.Instance.ExitSkill();
                });
            }
        }
    }
}