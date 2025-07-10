using System;
using System.Collections.Generic;
using IdleProject.Battle.Effect;
using IdleProject.Battle.Projectile;
using IdleProject.Core;
using Unity.Cinemachine;

namespace IdleProject.Battle.Character.Skill.SkillAction.Implement
{
    public class ProjectileAttack : ISkillAction
    {
        private readonly Func<BattleEffect> _getHitEffect;
        private readonly Func<BattleProjectile> _getProjectile;
        private readonly float _projectileSpeed;
        private readonly IEnumerator<float> _skillValues;

        public ProjectileAttack(Func<BattleEffect> getHitEffect, Func<BattleProjectile> getProjectile,
            IEnumerable<float> skillValues, float projectileSpeed)
        {
            _getHitEffect = getHitEffect;
            _getProjectile = getProjectile;
            _projectileSpeed = projectileSpeed;
            _skillValues = skillValues.GetEnumerator();
            _skillValues.MoveNext();
        }
        
        public void ExecuteSkillAction(CharacterController controller, IEnumerable<CharacterController> targetList)
        {
            var attackDamage = controller.StatSystem.GetStatValue(CharacterStatType.AttackDamage) * _skillValues.Current;
            
            foreach (var target in targetList)
            {
                var projectile = _getProjectile.Invoke();
                projectile.transform.position = controller.offset.GetOffsetTransform(CharacterOffsetType.ProjectileOffset).position;
                projectile.Target = target;
                projectile.projectileSpeed = _projectileSpeed;
                projectile.hitEvent.AddListener((hitTarget) =>
                {
                    controller.Hit(hitTarget, attackDamage);
                    
                    var attackHitEffect = _getHitEffect?.Invoke();
                    if (attackHitEffect)
                    {
                        attackHitEffect.transform.position = target.HitEffectOffset;
                        attackHitEffect.transform.rotation = controller.transform.rotation;
                    }
                });
            }
        }
    }
}