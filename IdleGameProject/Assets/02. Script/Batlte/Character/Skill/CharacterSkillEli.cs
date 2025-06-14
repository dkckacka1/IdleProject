using System;
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
            var skillProjectile = Controller.GetSkillProjectile?.Invoke();

            if (skillProjectile)
            {
                skillProjectile.transform.position = Controller.offset.GetProjectilePosition;
                skillProjectile.Target = target;
                skillProjectile.hitEvent.AddListener(takeDamagedAble =>
                {
                    Controller.Hit(takeDamagedAble, attackDamage);
                    
                    var attackHitEffect = Controller.GetAttackHitEffect?.Invoke();
                    if (attackHitEffect) 
                        attackHitEffect.transform.position = takeDamagedAble.HitEffectOffset;
                    
                    GameManager.GetCurrentSceneManager<BattleManager>().ExitSkill();
                });
            }
        }
    }
}