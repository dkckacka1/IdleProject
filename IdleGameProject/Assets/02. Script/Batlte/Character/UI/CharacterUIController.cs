using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.UI;
using UnityEngine;

namespace IdleProject.Battle.UI
{
    public class CharacterUIController : MonoBehaviour
    {
        private HealthBar _fluidHealthBar;
        private CharacterOffset _offset;

        protected static BattleUIController GetBattleUI => UIManager.Instance.GetUIController<BattleUIController>();

        private void Awake()
        {
            _offset = GetComponent<CharacterOffset>();
        }

        public virtual void Initialized(CharacterData data, StatSystem stat)
        {
            SetFluidHealthBar(stat).Forget();
        }

        public virtual void OnBattleUIEvent()
        {
            _fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(_offset.FluidHealthBarOffset));
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
            var textPosition = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(_offset.BattleTextOffset) + randomPos);
            battleText.ShowText(textPosition, text);
        }
        private async UniTaskVoid SetFluidHealthBar(StatSystem characterStat)
        {
            _fluidHealthBar = await ResourcesLoader.InstantiateUI<HealthBar>(SceneType.Battle, "FluidHealthBar");
            _fluidHealthBar.transform.SetParent(GetBattleUI.FluidHealthBarParent);
            _fluidHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            _fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(_offset.FluidHealthBarOffset));
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, _fluidHealthBar.OnChangeHealthPoint);
        }
    }
}