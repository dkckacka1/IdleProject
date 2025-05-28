using System.Collections.Generic;
using Engine.Core.Time;
using IdleProject.Core.ObjectPool;
using UnityEngine;

namespace IdleProject.Battle.Effect
{
    public class BattleEffect : MonoBehaviour, IPoolable
    {
        private List<ParticleSystem> _particleList;
        
        private void OnParticleSystemStopped()
        {
            var poolableObject = GetComponent<PoolableObject>();
            ObjectPoolManager.Instance.Release(poolableObject);
        }

        public void OnCreateAction()
        {
            _particleList = new List<ParticleSystem>(GetComponentsInChildren<ParticleSystem>());
        }

        public void OnGetAction()
        {
            OnTimeFactorChange(BattleManager.GetCurrentBattleSpeed);
            BattleManager.GetChangeBattleSpeedEvent.AddListener(OnTimeFactorChange);
        }

        public void OnReleaseAction()
        {
            transform.position = Vector3.zero;
            BattleManager.GetChangeBattleSpeedEvent.RemoveListener(OnTimeFactorChange);
        }

        private void OnTimeFactorChange(float timeFactor)
        {
            foreach (var particle in _particleList)
            {
                var main = particle.main;
                main.simulationSpeed = timeFactor;
            }
        }
    }
}