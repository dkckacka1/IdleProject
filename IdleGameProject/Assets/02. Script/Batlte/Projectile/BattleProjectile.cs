using IdleProject.Battle.Character;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.Battle.Projectile
{
    public class BattleProjectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float projectileSpeed;


        [HideInInspector] public UnityEvent<ITakeDamagedAble> hitEvent;
        [HideInInspector] public ITakeDamagedAble target;

        public void OnCreateAction()
        {
        }

        public void OnGetAction()
        {
            BattleManager.Instance.battleEvent.AddListener(OnBattleEvent);
        }

        public void OnReleaseAction()
        {
            hitEvent.RemoveAllListeners();
            target = null;
            BattleManager.Instance.battleEvent.RemoveListener(OnBattleEvent);
        }

        public void OnBattleEvent()
        {
            Vector3 directionVector = (target.HitEffectOffset - transform.position).normalized;
            transform.LookAt(target.HitEffectOffset);
            transform.position += directionVector * projectileSpeed * Time.deltaTime;
        }

        private void OnTriggerEnter(Collider other)
        {
            var takeAble = other.GetComponent<ITakeDamagedAble>();
            if (takeAble is not null && takeAble == target)
            {
                hitEvent.Invoke(target);
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            }
        }

        private void DistanceCheck()
        {
            float distance = Vector3.Distance(target.HitEffectOffset, this.transform.position);
            if (distance < 0.5f)
            {
                hitEvent.Invoke(target);
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            }
        }
    }
}
