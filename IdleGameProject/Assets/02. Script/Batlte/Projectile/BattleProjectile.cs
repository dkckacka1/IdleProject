using Engine.Core.Time;
using IdleProject.Battle.Character;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using IPoolable = IdleProject.Core.ObjectPool.IPoolable;

namespace IdleProject.Battle.Projectile
{
    public class BattleProjectile : MonoBehaviour, IPoolable
    {
        [Inject] 
        private BattleManager _battleManager;
        
        [SerializeField] private float projectileSpeed;


        [HideInInspector] public UnityEvent<ITakeDamagedAble> hitEvent;
        [HideInInspector] public ITakeDamagedAble Target;

        public void OnCreateAction()
        {
        }

        public void OnGetAction()
        {
            _battleManager.BattleObjectEventDic[BattleObjectType.Projectile].AddListener(OnBattleEvent);
        }

        public void OnReleaseAction()
        {
            hitEvent.RemoveAllListeners();
            Target = null;
            _battleManager.BattleObjectEventDic[BattleObjectType.Projectile].RemoveListener(OnBattleEvent);
        }

        private void OnBattleEvent()
        {
            var directionVector = (Target.HitEffectOffset - transform.position).normalized;
            transform.LookAt(Target.HitEffectOffset);
            transform.position += directionVector * projectileSpeed * BattleManager.GetCurrentBattleDeltaTime;
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
