using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Engine.Core.EventBus;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;
using IdleProject.Core.Sound;
using UnityEngine;
using UnityEngine.Events;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Battle.Effect
{
    public class BattleEffect : MonoBehaviour, IPoolable, IEnumEvent<BattleGameStateType>
    {
        private List<ParticleSystem> _particleList;

        public UnityEvent onBattleEvent = null;
        public UnityEvent releaseEvent = null;

        private BattleManager _battleManager;

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
            _battleManager.BattleObjectEventDic[BattleObjectType.Effect].AddListener(OnBattleEvent);
            SoundManager.Instance.PlaySfx(name);
        }

        public void OnReleaseAction()
        {
            releaseEvent.Invoke();
            releaseEvent.RemoveAllListeners();
            
            transform.position = Vector3.zero;
            _battleManager.GetChangeBattleSpeedEvent.RemoveListener(OnTimeFactorChange);
            _battleManager.GameStateEventBus.UnPublishEvent(this);
            _battleManager.BattleObjectEventDic[BattleObjectType.Effect].RemoveListener(OnBattleEvent);
            onBattleEvent.RemoveAllListeners();
        }
        
        public void SetSkillEffect()
        {
            _battleManager.AddSkillObject(this);
            releaseEvent.AddListener(() => _battleManager.RemoveSkillObject(this));
        }
        
        private void OnBattleEvent()
        {
            onBattleEvent?.Invoke();
        }

        public void LoopEffect(float loopTime)
        {
            _particleList.ForEach(particle =>
            {
                var particleMain = particle.main;
                particleMain.loop = true;
            });
            
            LoopTimer(loopTime, this).Forget();
            return;

            async UniTaskVoid LoopTimer(float time, BattleEffect loopEffect)
            {
                await _battleManager.GetBattleTimer(time);
                if (!loopEffect || !loopEffect.gameObject)
                    return;
                
                var poolableObject = GetComponent<PoolableObject>();
                ObjectPoolManager.Instance.Release(poolableObject);
            }
        }

        public void OneShotEffect()
        {
            _particleList.ForEach(particle =>
            {
                var particleMain = particle.main;
                particleMain.loop = false;
            });
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