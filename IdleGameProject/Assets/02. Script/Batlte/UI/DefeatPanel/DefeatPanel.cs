using Cysharp.Threading.Tasks;
using DG.Tweening;
using IdleProject.Core;
using IdleProject.Core.UI;
using IdleProject.Lobby.UI.CharacterPopup;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.UI
{
    public class DefeatPanel : UIPanel
    {
        [SerializeField] private RectTransform title;
        [SerializeField] private CanvasGroup reinforce;
        [SerializeField] private CanvasGroup buttonsObject;
        
        [BoxGroup("OpenTween"), SerializeField] private float openActiveInterval;
        [BoxGroup("OpenTween"), SerializeField] private float titleDuration;
        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CharacterReinForceButton").Button.onClick.AddListener(GoToLobbyAtCharacterReinforce);
            UIManager.Instance.GetUI<UIButton>("EquipmentReinForceButton").Button.onClick.AddListener(GoToLobbyAtEquipmentReinforce);
            UIManager.Instance.GetUI<UIButton>("DefeatPanelRetryStageButton").Button.onClick.AddListener(RetryStage);
            UIManager.Instance.GetUI<UIButton>("DefeatPanelExitButton").Button.onClick.AddListener(GotoLobby);
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            var openSequence = DOTween.Sequence();
            openSequence.OnStart(() =>
            {
                reinforce.gameObject.SetActive(false);
                buttonsObject.gameObject.SetActive(false);
                 
                title.anchoredPosition = new Vector3(0, 750, 0);
            });
            openSequence.Append(title.DOAnchorPosY(400, titleDuration).SetEase(Ease.OutBack));
            openSequence.AppendInterval(openActiveInterval);
            openSequence.AppendCallback(() =>
            {
                reinforce.gameObject.SetActive(true);
                reinforce.alpha = 0f;
                reinforce.DOFade(1f, 0.2f);
            });
            openSequence.AppendInterval(openActiveInterval);
            openSequence.AppendCallback(() =>
            {
                buttonsObject.gameObject.SetActive(true);
                buttonsObject.alpha = 0f;
                buttonsObject.DOFade(1f, 0.2f);
            });
        }

        private void GoToLobbyAtCharacterReinforce()
        {
            GameManager.Instance.LoadScene(SceneType.Lobby, () =>
            {
                UIManager.Instance.GetUI<CharacterPanel>().OpenPanel();
                UIManager.Instance.GetUI<SelectSlotPanel>().OpenPanel(SlotPanelType.Character);
            }).Forget();
        }

        private void GoToLobbyAtEquipmentReinforce()
        {
            GameManager.Instance.LoadScene(SceneType.Lobby, () =>
            {
                UIManager.Instance.GetUI<CharacterPanel>().OpenPanel();
                UIManager.Instance.GetUI<SelectSlotPanel>().OpenPanel(SlotPanelType.EquipmentItem);
            }).Forget();
        }

        private void RetryStage()
        {
            GameManager.Instance.LoadScene(SceneType.Battle);
        }

        private void GotoLobby()
        {
            GameManager.Instance.LoadScene(SceneType.Lobby);
        }
    }
}