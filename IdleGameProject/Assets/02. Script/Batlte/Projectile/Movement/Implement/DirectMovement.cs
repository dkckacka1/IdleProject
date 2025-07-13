using UnityEngine;

namespace IdleProject.Battle.Projectile.Movement.Implement
{
    public class DirectMovement : IProjectileMovement
    {
        private readonly float _projectileSpeed;

        public DirectMovement(float projectileSpeed)
        {
            _projectileSpeed = projectileSpeed;
        }

        public void ProjectileMove(BattleProjectile projectile, Transform targetTransform, float deltaTime)
        {
            projectile.transform.LookAt(targetTransform);
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, targetTransform.position, _projectileSpeed * deltaTime);
        }
    }
}