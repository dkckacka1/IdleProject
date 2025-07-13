using UnityEngine;

namespace IdleProject.Battle.Projectile.Movement
{
    public interface IProjectileMovement
    {
        public void Initialize();
        public void ProjectileMove(BattleProjectile projectile, Transform targetTransform, float deltaTime);
    }
}