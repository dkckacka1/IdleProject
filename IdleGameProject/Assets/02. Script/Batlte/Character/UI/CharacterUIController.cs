using System;
using Cysharp.Threading.Tasks;
using Engine.Core.EventBus;
using IdleProject.Battle.Character;
using IdleProject.Battle.Character.EventGroup;
using IdleProject.Core;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Data.StaticData;
using UnityEngine;
using Random = UnityEngine.Random;

namespace IdleProject.Battle.UI
{
    public class CharacterUIController : MonoBehaviour, IEnumEvent<BattleStateType>, IEventGroup<BattleManager>
    {
        private HealthBar _fluidHealthBar;
        private CharacterOffset _offset;

        private Camera _mainCamera;
        
        protected static BattleUIController GetBattleUI => UIManager.Instance.GetUIController<BattleUIController>();

        public virtual void Initialized(StaticCharacterData data, StatSystem stat)
        {
            _offset = GetComponent<CharacterOffset>();
            
            _mainCamera = Camera.main;

            SetFluidHealthBar(stat).Forget();
        }

        protected virtual void OnBattleUIEvent()
        {
            _fluidHealthBar.transform.position =
                UIManager.GetUIInScreen(_mainCamera.WorldToScreenPoint(_offset.GetOffsetTransform(CharacterOffsetType.FluidHealthBarOffset).position));
            _fluidHealthBar.PlayDamageSlider();
        }

        public void OnCharacterDeath()
        {
            _fluidHealthBar.gameObject.SetActive(false);
        }

        public virtual void OnCharacterRemove()
        {
            Destroy(_fluidHealthBar.gameObject);
        }

        public void ShowBattleText(string text)
        {
            var battleText = GetBattleUI.GetBattleText.Invoke();
            Vector3 randomPos = Random.insideUnitCircle * _offset.BattleTextOffsetRadius * 50f;
            var textPosition =
                UIManager.GetUIInScreen(_mainCamera.WorldToScreenPoint(_offset.GetOffsetTransform(CharacterOffsetType.FluidHealthBarOffset).position) + randomPos);
            battleText.ShowText(textPosition, text);
        }

        public void OnEnumChange(BattleStateType type)
        {
            switch (type)
            {
                case BattleStateType.Ready:
                    break;
                case BattleStateType.Battle:
                    _fluidHealthBar.gameObject.SetActive(true);
                    break;
                case BattleStateType.Skill:
                    break;
                case BattleStateType.Win:
                    _fluidHealthBar.gameObject.SetActive(false);
                    break;
                case BattleStateType.Defeat:
                    _fluidHealthBar.gameObject.SetActive(false);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public void Publish(BattleManager publisher)
        {
            publisher.BattleStateEventBus.PublishEvent(this);
            publisher.BattleObjectEventDic[BattleObjectType.UI].AddListener(OnBattleUIEvent);
        }

        public void UnPublish(BattleManager publisher)
        {
            publisher.BattleStateEventBus.UnPublishEvent(this);
            publisher.BattleObjectEventDic[BattleObjectType.UI].RemoveListener(OnBattleUIEvent);
        }
        
        private async UniTaskVoid SetFluidHealthBar(StatSystem characterStat)
        {
            var obj = ResourceManager.Instance.GetPrefab(ResourceManager.UIPrefab, "FluidHealthBar").GetComponent<HealthBar>();
            var healthBars = await InstantiateAsync(obj, GetBattleUI.FluidHealthBarParent).ToUniTask();
            _fluidHealthBar = healthBars[0];
            _fluidHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            _fluidHealthBar.transform.position =
                UIManager.GetUIInScreen(_mainCamera.WorldToScreenPoint(_offset.GetOffsetTransform(CharacterOffsetType.FluidHealthBarOffset).position));
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, _fluidHealthBar.OnChangeHealthPoint);
            _fluidHealthBar.gameObject.SetActive(false);
        }
    }
}