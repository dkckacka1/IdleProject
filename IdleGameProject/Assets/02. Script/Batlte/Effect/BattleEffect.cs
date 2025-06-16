using System;
using System.Collections.Generic;
using Engine.Core.EventBus;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace IdleProject.Battle.Effect
{
    public class BattleEffect : MonoBehaviour, IPoolable, IEnumEvent<GameStateType>
    {
        private List<ParticleSystem> _particleList;

        public UnityEvent<ITakeDamagedAble> effectTriggerEnterEvent = null;
        public UnityEvent releaseEvent = null;

        private void OnTriggerEnter(Collider other)
        {
            var takeDamageAble = other.GetComponent<ITakeDamagedAble>();
            if (takeDamageAble is not null)
            {
                effectTriggerEnterEvent?.Invoke(takeDamageAble);
            }
        }

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
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.PublishEvent(this);
        }

        public void OnReleaseAction()
        {
            releaseEvent.Invoke();
            releaseEvent.RemoveAllListeners();
            
            transform.position = Vector3.zero;
            BattleManager.GetChangeBattleSpeedEvent.RemoveListener(OnTimeFactorChange);
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.RemoveEvent(this);
        }
        
        public void SetSkillEffect()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().AddSkillObject(this);
            releaseEvent.AddListener(() => GameManager.GetCurrentSceneManager<BattleManager>().RemoveSkillObject(this));
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