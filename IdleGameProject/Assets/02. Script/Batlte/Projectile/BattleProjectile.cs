using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.Battle.Projectile
{
    public class BattleProjectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float projectileSpeed;

        [HideInInspector] public UnityEvent<ITakeDamagedAble> hitEvent;
        [HideInInspector] public ITakeDamagedAble Target;
        [HideInInspector] public UnityEvent releaseEvent = null;

        public void OnCreateAction()
        {
        }

        public void OnGetAction()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().BattleObjectEventDic[BattleObjectType.Projectile].AddListener(OnBattleEvent);
        }

        public void OnReleaseAction()
        {
            releaseEvent.Invoke();
            releaseEvent.RemoveAllListeners();
            hitEvent.RemoveAllListeners();
            Target = null;
            GameManager.GetCurrentSceneManager<BattleManager>().BattleObjectEventDic[BattleObjectType.Projectile].RemoveListener(OnBattleEvent);
        }

        public void SetSkillProjectile()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().AddSkillObject(this);
            releaseEvent.AddListener(() => GameManager.GetCurrentSceneManager<BattleManager>().RemoveSkillObject(this));
        }

        private void OnBattleEvent()
        {
            var directionVector = (Target.HitEffectOffset - transform.position).normalized;
            transform.LookAt(Target.HitEffectOffset);
            transform.position += directionVector * projectileSpeed * GameManager.GetCurrentSceneManager<BattleManager>().GetCurrentBattleDeltaTime;
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
