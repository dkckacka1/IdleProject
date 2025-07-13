using System;
using System.Collections.Generic;
using IdleProject.Battle.Projectile;
using IdleProject.Battle.Projectile.Movement;
using IdleProject.Battle.Projectile.Movement.Implement;
using IdleProject.Core;
using IdleProject.Data.BehaviourData;

namespace IdleProject.Battle.Character.Behaviour.SkillAction.Implement
{
    public class ProjectileAction : BehaviourAction
    {
        private readonly List<IBehaviourAction> _onHitActionList = new();
        private readonly Func<BattleProjectile> _getBattleProjectile;
        private readonly ProjectileMoveType _projectileMoveType;
        private readonly CharacterOffsetType _projectileCreateOffset;
        private readonly CharacterOffsetType _projectileTargetingOffset;

        private readonly IProjectileMovement _projectileMovement;
        
        public ProjectileAction(ProjectileBehaviourActionData behaviourActionData, CharacterController controller) : base(behaviourActionData, controller)
        {
            _getBattleProjectile = GameManager.GetCurrentSceneManager<BattleManager>().GetPoolable<BattleProjectile>(PoolableType.BattleEffect, behaviourActionData.projectileObjectName);
            _projectileMoveType = behaviourActionData.projectileMoveType;
            _projectileCreateOffset = behaviourActionData.projectileCreateOffset;
            _projectileTargetingOffset = behaviourActionData.projectileTargetingOffset;
            
            foreach (var onHitAction in behaviourActionData.projectileOnHitAction)
            {
                _onHitActionList.Add(GetSkillAction(onHitAction, controller));
            }
            
            _projectileMovement = GetProjectileMovement(behaviourActionData);
        }

        public override void ActionExecute(bool isSkillBehaviour)
        {
            _projectileMovement.Initialize();
            
            foreach (var target in GetTargetList.Invoke())
            {
                // 대상에게 발사
                var projectile = CreateBattleProjectile();
                projectile.target = target.offset.GetOffsetTransform(_projectileTargetingOffset);
                projectile.Movement = _projectileMovement;
                projectile.hitEvent.AddListener(() =>
                {
                    // 맞추면 OnHitAction 발동
                    foreach (var onHitAction in _onHitActionList)
                    {
                        onHitAction.SetTarget(target);
                        onHitAction.ActionExecute(isSkillBehaviour);
                    }
                });

                if (isSkillBehaviour)
                {
                    projectile.SetSkillProjectile();
                }
            }
        }

        private IProjectileMovement GetProjectileMovement(ProjectileBehaviourActionData behaviourActionData)
        {
            return _projectileMoveType switch
            {
                ProjectileMoveType.Direct => new DirectMovement(behaviourActionData.projectileSpeed),
                ProjectileMoveType.Howitzer => new HowitzerMovement(behaviourActionData.projectileSpeed, behaviourActionData.arcHeight),
                _ => throw new ArgumentOutOfRangeException()
            };
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