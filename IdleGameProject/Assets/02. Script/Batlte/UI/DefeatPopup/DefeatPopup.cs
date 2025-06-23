using DG.Tweening;
using IdleProject.Core;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Battle.UI
{
    public class DefeatPopup : UIPopup
    {
        [SerializeField] private RectTransform title;
        [SerializeField] private CanvasGroup reinforce;
        [SerializeField] private CanvasGroup buttonsObject;
        
        [BoxGroup("OpenTween"), SerializeField] private float openActiveInterval;
        [BoxGroup("OpenTween"), SerializeField] private float titleDuration;
        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CharacterReinForceButton").Button.onClick.AddListener(GotoLobbyAtCharacterReinforce);
            UIManager.Instance.GetUI<UIButton>("EquipmentReinForceButton").Button.onClick.AddListener(GotoLobbyAtEquipmentReinforce);
            UIManager.Instance.GetUI<UIButton>("DefeatPopupRetryStageButton").Button.onClick.AddListener(RetryStage);
            UIManager.Instance.GetUI<UIButton>("DefeatPopupExitButton").Button.onClick.AddListener(GotoLobby);
        }

        public override void OpenPopup()
        {
            base.OpenPopup();
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

        private void GotoLobbyAtCharacterReinforce()
        {
            
        }

        private void GotoLobbyAtEquipmentReinforce()
        {
            
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