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
        private CharacterOffset offset;

        protected static BattleUIController GetBattleUI => UIManager.Instance.GetUIController<BattleUIController>();

        private void Awake()
        {
            offset = GetComponent<CharacterOffset>();
        }

        public virtual void Initialized(CharacterData data, StatSystem stat)
        {
            SetFluidHealthBar(stat).Forget();
        }

        public virtual void OnBattleUIEvent()
        {
            fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(offset.FluidHealthBarOffset));
            fluidHealthBar.PlayDamageSlider();
        }

        public virtual void OnCharacterDeath()
        {
            fluidHealthBar.gameObject.SetActive(false);
        }

        public void ShowBattleText(string text)
        {
            var battleText = GetBattleUI.GetBattleText.Invoke();
            Vector3 randomPos = Random.insideUnitCircle * offset.BattleTextOffsetRadius;
            var textPosition = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(offset.BattleTextOffset) + randomPos);
            battleText.ShowText(textPosition, text);
        }
        private async UniTaskVoid SetFluidHealthBar(StatSystem characterStat)
        {
            fluidHealthBar = await ResourcesLoader.InstantiateUI<HealthBar>(SceneType.Battle, "FluidHealthBar");
            fluidHealthBar.transform.SetParent(GetBattleUI.FluidHealthBarParent);
            fluidHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(offset.FluidHealthBarOffset));
            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, fluidHealthBar.OnChangeHealthPoint);
        }
    }
}