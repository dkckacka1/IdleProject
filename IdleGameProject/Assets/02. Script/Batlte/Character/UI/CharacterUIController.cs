using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.UI;
using UnityEngine;
using Zenject;

namespace IdleProject.Battle.UI
{
    public class CharacterUIController : MonoBehaviour
    {
        [Inject] private BattleManager _battleManager;
        [Inject] private BattleResourceLoader _battleResourceLoader;
        [Inject] protected BattleUIController BattleUIController;

        private HealthBar _fluidHealthBar;
        private CharacterOffset _offset;

        [Inject]
        public virtual void Initialized(CharacterData data, StatSystem stat, CharacterOffset offset)
        {
            _offset = offset;
            
            SetFluidHealthBar(stat).Forget();
            _battleManager.BattleObjectEventDic[BattleObjectType.UI].AddListener(OnBattleUIEvent);
        }

        public virtual void OnBattleUIEvent()
        {
            _fluidHealthBar.transform.position =
                UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(_offset.FluidHealthBarOffset));
            _fluidHealthBar.PlayDamageSlider();
        }

        public virtual void OnCharacterDeath()
        {
            _fluidHealthBar.gameObject.SetActive(false);
        }

        private async UniTaskVoid SetFluidHealthBar(StatSystem characterStat)
        {
            _fluidHealthBar = await _battleResourceLoader.InstantiateUI<HealthBar>(SceneType.Battle, "FluidHealthBar");
            _fluidHealthBar.transform.SetParent(BattleUIController.FluidHealthBarParent);
            _fluidHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            _fluidHealthBar.transform.position =
                UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(_offset.FluidHealthBarOffset));
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, _fluidHealthBar.OnChangeHealthPoint);
        }
    }
}