using IdleProject.Core.ObjectPool;
using UnityEngine;

namespace IdleProject.Battle.Effect
{
    public class BattleEffect : MonoBehaviour, IPoolable
    {
        private void OnParticleSystemStopped()
        {
            var poolableObject = GetComponent<PoolableObject>();
            ObjectPoolManager.Instance.Release(poolableObject);
        }

        public void OnCreateAction()
        {
        }

        public void OnGetAction()
        {
        }

        public void OnReleaseAction()
        {
        }
    }
}