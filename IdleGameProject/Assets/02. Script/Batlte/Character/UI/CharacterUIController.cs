using Cysharp.Threading.Tasks;
using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.UI
{
    public class CharacterUIController : MonoBehaviour
    {
        [BoxGroup("FluidHealthBar"), SerializeField] Vector3 fluidHealthBarOffset;
        private FluidHealthBar fluidHealthBar;

        [BoxGroup("BattleText"), SerializeField] Vector3 battleTextOffset;
        [BoxGroup("BattleText"), SerializeField] float battleTextRadius = 70f;



        public async UniTask SpawnCharacterUI()
        {
            fluidHealthBar = await ResourcesLoader.InstantiateUI<FluidHealthBar>(SceneType.Battle);
            fluidHealthBar.transform.SetParent(UIManager.Instance.GetUIController<BattleUIController>().FluidHealthBarParent);
        }

        public virtual void SetCharacterUI(StatSystem characterStat)
        {
            fluidHealthBar.Initialized(characterStat.GetStatValue(CharacterStatType.HealthPoint, true));
            fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(transform.position) + fluidHealthBarOffset);

            characterStat.PublishValueChangedEvent(CharacterStatType.HealthPoint, fluidHealthBar.ChangeCurrentHealthPoint);
        }

        public void OnBattleUIEvent()
        {
            fluidHealthBar.transform.position = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(transform.position) + fluidHealthBarOffset);
            fluidHealthBar.PlayDamageSlider();
        }

        public void ShowBattleText(string text)
        {
            var battleText = UIManager.Instance.GetUIController<BattleUIController>().GetBattleText.Invoke();
            Vector3 randomPos = Random.insideUnitCircle * battleTextRadius;
            var textPosition = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(transform.position) + battleTextOffset + randomPos);
            battleText.ShowText(textPosition, text);
        }
    }
}