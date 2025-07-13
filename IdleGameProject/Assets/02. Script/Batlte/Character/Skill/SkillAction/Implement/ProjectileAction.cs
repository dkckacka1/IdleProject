using System;
using System.Collections.Generic;
using IdleProject.Battle.Effect;
using IdleProject.Battle.Projectile;
using IdleProject.Core;
using IdleProject.Data.SkillData;

namespace IdleProject.Battle.Character.Skill.SkillAction.Implement
{
    public class ProjectileAction : BehaviourAction
    {
        private readonly List<IBehaviourAction> _onHitActionList = new List<IBehaviourAction>();
        private readonly Func<BattleProjectile> _getBattleProjectile;
        private readonly float _projectileSpeed; 
        private ProjectileMoveType _projectileMoveType;
        private readonly CharacterOffsetType _projectileCreateOffset;
        private CharacterOffsetType _projectileTargetingOffset;
        
        public ProjectileAction(ProjectileSkillActionData skillActionData, CharacterController controller) : base(skillActionData, controller)
        {
            _getBattleProjectile = GameManager.GetCurrentSceneManager<BattleManager>().GetPoolable<BattleProjectile>(PoolableType.BattleEffect, skillActionData.projectileObjectName);
            _projectileSpeed = skillActionData.projectileSpeed;
            _projectileMoveType = skillActionData.projectileMoveType;
            _projectileCreateOffset = skillActionData.projectileCreateOffset;
            _projectileTargetingOffset = skillActionData.projectileTargetingOffset;
            
            foreach (var onHitAction in skillActionData.projectileOnHitAction)
            {
                _onHitActionList.Add(GetSkillAction(onHitAction, controller));
            }
        }

        public override void ActionExecute()
        {
            foreach (var target in GetTargetList.Invoke())
            {
                // 대상에게 발사
                var projectile = CreateBattleProjectile();
                projectile.target = target;
                projectile.projectileSpeed = _projectileSpeed;
                projectile.hitEvent.AddListener(() =>
                {
                    // 맞추면 OnHitAction 발동
                    foreach (var onHitAction in _onHitActionList)
                    {
                        onHitAction.SetTarget(target);
                        onHitAction.ActionExecute();
                    }
                });
            }
        }

        private BattleProjectile CreateBattleProjectile()
        {
            var projectile = _getBattleProjectile.Invoke();
            SetProjectilePosition(projectile, _projectileCreateOffset);
            
            return projectile;
        }

        private void SetProjectilePosition(BattleProjectile projectile, CharacterOffsetType createOffset)
        {
            projectile.transform.position = Controller.offset.GetOffsetTransform(createOffset).position;
        }
    }
}