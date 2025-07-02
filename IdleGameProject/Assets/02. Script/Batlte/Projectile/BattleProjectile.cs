using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.Battle.Projectile
{
    public class BattleProjectile : MonoBehaviour, IPoolable
    {
        [HideInInspector] public UnityEvent<ITakeDamagedAble> hitEvent;
        [HideInInspector] public ITakeDamagedAble Target;
        [HideInInspector] public UnityEvent releaseEvent = null;
        
        [SerializeField] private ProjectileInfo projectileInfo;

        private BattleManager _battleManager;
        private Vector3 _targetPosition;

        public void OnCreateAction()
        {
            _battleManager = GameManager.GetCurrentSceneManager<BattleManager>();
        }

        public void OnGetAction()
        {
            _battleManager.BattleObjectEventDic[BattleObjectType.Projectile].AddListener(OnBattleEvent);
        }

        public void OnReleaseAction()
        {
            releaseEvent.Invoke();
            releaseEvent.RemoveAllListeners();
            hitEvent.RemoveAllListeners();
            Target = null;
            _battleManager.BattleObjectEventDic[BattleObjectType.Projectile].RemoveListener(OnBattleEvent);
        }

        public void SetSkillProjectile()
        {
            _battleManager.AddSkillObject(this);
            releaseEvent.AddListener(() => _battleManager.RemoveSkillObject(this));
        }

        private void OnBattleEvent()
        {
            var directionVector = (Target.HitEffectOffset - transform.position).normalized;
            transform.LookAt(Target.HitEffectOffset);
            transform.position += directionVector * projectileInfo.projectileSpeed * _battleManager.GetCurrentBattleDeltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            var takeAble = other.GetComponent<ITakeDamagedAble>();
            if (takeAble is not null && takeAble == Target)
            {
                hitEvent.Invoke(Target);
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            }
        }

        private void DistanceCheck()
        {
            var distance = Vector3.Distance(Target.HitEffectOffset, this.transform.position);
            if (distance < 0.5f)
            {
                hitEvent.Invoke(Target);
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            }
        }
    }
}