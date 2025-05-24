using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.UI;
using UnityEngine;

namespace IdleProject.Battle.UI
{
    public class CharacterUIController : MonoBehaviour
    {
        private HealthBar fluidHealthBar;
        protected CharacterUIOffsetValue uiOffset;

        protected static BattleUIController GetBattleUI => UIManager.Instance.GetUIController<BattleUIController>();

        public virtual void Initialized(CharacterData data, StatSystem stat)
        {
            uiOffset = data.uiOffset;
            SetFluidHealthBar(stat).Forget();
        }

        public virtual void OnBattleUIEvent()
        {
            fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(transform.position) + uiOffset.fluidHealthBarOffset);
            fluidHealthBar.PlayDamageSlider();
        }

        public void ShowBattleText(string text)
        {
            var battleText = GetBattleUI.GetBattleText.Invoke();
            Vector3 randomPos = Random.insideUnitCircle * uiOffset.battleTextRadius;
            var textPosition = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(transform.position) + uiOffset.battleTextOffset + randomPos);
            battleText.ShowText(textPosition, text);
        }

        private async UniTaskVoid SetFluidHealthBar(StatSystem characterStat)
        {
            fluidHealthBar = await ResourcesLoader.InstantiateUI<HealthBar>(SceneType.Battle, "FluidHealthBar");
            fluidHealthBar.transform.SetParent(GetBattleUI.FluidHealthBarParent);
            fluidHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(transform.position) + uiOffset.fluidHealthBarOffset);
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, fluidHealthBar.ChangeCurrentHealthPoint);
        }
    }
}