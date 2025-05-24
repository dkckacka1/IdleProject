using IdleProject.Battle.Character;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.Battle.Projectile
{
    public class BattleProjectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private Transform hitTransform;
        [SerializeField] private float projectileSpeed;
        [SerializeField] private float hitRange = 0.3f;


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
            Vector3 directionVector = (target.HitEffectTransform.position - transform.position).normalized;
            transform.LookAt(target.HitEffectTransform);
            transform.position += directionVector * projectileSpeed * Time.deltaTime;

            if (Vector3.Distance(hitTransform.position, target.HitEffectTransform.position) < hitRange)
            {
                hitEvent.Invoke(target);
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            }
        }
    }
}
