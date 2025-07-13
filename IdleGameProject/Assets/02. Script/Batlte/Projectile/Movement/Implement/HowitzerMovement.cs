using UnityEngine;

namespace IdleProject.Battle.Projectile.Movement.Implement
{
    public class HowitzerMovement : IProjectileMovement
    {
        private readonly float _projectileSpeed;
        private readonly float _arcHeight;

        private Vector3 _startPosition;
        private float _progress;

        private bool _initialized = false;

        public HowitzerMovement(float projectileSpeed, float arcHeight)
        {
            _projectileSpeed = projectileSpeed;
            _arcHeight = arcHeight;
        }

        public void Initialize()
        {
            _progress = 0f;
            _initialized = false;
        }
        
        public void ProjectileMove(BattleProjectile projectile, Transform targetTransform, float deltaTime)
        {
            if (!_initialized)
            {
                _startPosition = projectile.transform.position;
                _initialized = true;
            }

            var totalDistance = Vector3.Distance(_startPosition, targetTransform.position);
            var moveDelta = _projectileSpeed * deltaTime;
            _progress += moveDelta / totalDistance;
            _progress = Mathf.Clamp01(_progress);

            var linearPos = Vector3.Lerp(_startPosition, targetTransform.position, _progress);
            var heightOffset = _arcHeight * (_progress - _progress * _progress); // 포물선 y 오프셋
            linearPos.y += heightOffset;

            projectile.transform.position = linearPos;

            // 방향 예측을 위해 약간 앞을 바라보게
            var lookAheadProgress = Mathf.Min(_progress + 0.01f, 1f);
            var lookPos = Vector3.Lerp(_startPosition, targetTransform.position, lookAheadProgress);
            lookPos.y += _arcHeight * (lookAheadProgress - lookAheadProgress * lookAheadProgress);
            projectile.transform.LookAt(lookPos);
        }
    }
}