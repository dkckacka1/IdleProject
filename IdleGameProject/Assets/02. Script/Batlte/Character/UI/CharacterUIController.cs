using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.UI;
using IdleProject.Data;
using UnityEngine;

namespace IdleProject.Battle.UI
{
    public class CharacterUIController : MonoBehaviour
    {
        private HealthBar _fluidHealthBar;
        private CharacterOffset _offset;

        protected static BattleUIController GetBattleUI => UIManager.Instance.GetUIController<BattleUIController>();

        public virtual void Initialized(CharacterData data, StatSystem stat)
        {
            _offset = GetComponent<CharacterOffset>();
            
            SetFluidHealthBar(stat).Forget();
        }

        public virtual void OnBattleUIEvent()
        {
            _fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(_offset.GetFluidHealthBarPosition));
            _fluidHealthBar.PlayDamageSlider();
        }

        public virtual void OnCharacterDeath()
        {
            _fluidHealthBar.gameObject.SetActive(false);
        }

        public void ShowBattleText(string text)
        {
            var battleText = GetBattleUI.GetBattleText.Invoke();
            Vector3 randomPos = Random.insideUnitCircle * _offset.BattleTextOffsetRadius;
            var textPosition = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(_offset.GetHitEffectPosition) + randomPos);
            battleText.ShowText(textPosition, text);
        }
        private async UniTaskVoid SetFluidHealthBar(StatSystem characterStat)
        {
            _fluidHealthBar = await ResourceLoader.InstantiateUI<HealthBar>(SceneType.Battle, "FluidHealthBar");
            _fluidHealthBar.transform.SetParent(GetBattleUI.FluidHealthBarParent);
            _fluidHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            _fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(_offset.GetFluidHealthBarPosition));
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, _fluidHealthBar.OnChangeHealthPoint);
        }
    }
}