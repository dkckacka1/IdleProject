using System;
using System.Collections.Generic;
using Engine.Core.EventBus;
using Engine.Core.Time;
using IdleProject.Core.ObjectPool;
using UnityEngine;

namespace IdleProject.Battle.Effect
{
    public class BattleEffect : MonoBehaviour, IPoolable, IEnumEvent<GameStateType>
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
            BattleManager.Instance.GameStateEventBus.PublishEvent(this);
        }

        public void OnReleaseAction()
        {
            transform.position = Vector3.zero;
            BattleManager.GetChangeBattleSpeedEvent.RemoveListener(OnTimeFactorChange);
            BattleManager.Instance.GameStateEventBus.RemoveEvent(this);
        }

        private void OnTimeFactorChange(float timeFactor)
        {
            foreach (var particle in _particleList)
            {
                var main = particle.main;
                main.simulationSpeed = timeFactor;
            }
        }

        public void OnEnumChange(GameStateType type)
        {
            switch (type)
            {
                case GameStateType.Play:
                    foreach (var particle in _particleList)
                    {
                        particle.Play();
                    }
                    break;
                case GameStateType.Pause:
                    foreach (var particle in _particleList)
                    {
                        particle.Pause();
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
        
    }
}