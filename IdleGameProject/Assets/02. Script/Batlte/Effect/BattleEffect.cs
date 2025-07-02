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
    public class BattleEffect : MonoBehaviour, IPoolable, IEnumEvent<BattleGameStateType>
    {
        private List<ParticleSystem> _particleList;

        public UnityEvent<ITakeDamagedAble> effectTriggerEnterEvent = null;
        public UnityEvent releaseEvent = null;

        private BattleManager _battleManager;

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
            _battleManager = GameManager.GetCurrentSceneManager<BattleManager>();
        }

        public void OnGetAction()
        {
            OnTimeFactorChange(_battleManager.GetCurrentBattleSpeed);
            _battleManager.GetChangeBattleSpeedEvent.AddListener(OnTimeFactorChange);
            _battleManager.GameStateEventBus.PublishEvent(this);
        }

        public void OnReleaseAction()
        {
            releaseEvent.Invoke();
            releaseEvent.RemoveAllListeners();
            
            transform.position = Vector3.zero;
            _battleManager.GetChangeBattleSpeedEvent.RemoveListener(OnTimeFactorChange);
            _battleManager.GameStateEventBus.UnPublishEvent(this);
        }
        
        public void SetSkillEffect()
        {
            _battleManager.AddSkillObject(this);
            releaseEvent.AddListener(() => _battleManager.RemoveSkillObject(this));
        }

        private void OnTimeFactorChange(float timeFactor)
        {
            foreach (var particle in _particleList)
            {
                var main = particle.main;
                main.simulationSpeed = timeFactor;
            }
        }

        public void OnEnumChange(BattleGameStateType type)
        {
            switch (type)
            {
                case BattleGameStateType.Play:
                    foreach (var particle in _particleList)
                    {
                        particle.Play();
                    }
                    break;
                case BattleGameStateType.Pause:
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