using DG.Tweening;
using IdleProject.Core;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Battle.UI
{
    public class PausePopup : UIPopup
    {
        [BoxGroup("Background"), SerializeField]
        private Image backgroundImage;

        [BoxGroup("Background"), SerializeField]
        private float backgroundShadowDuration;

        [BoxGroup("Background"), SerializeField, Range(0f, 1f)]
        private float shadowAlphaValue;

        [BoxGroup("PopupBase"), SerializeField]
        private RectTransform popupBaseObj;

        [BoxGroup("PopupBase"), SerializeField]
        private float popupBaseTweenDuration;

        private float PopupBaseHeight => popupBaseObj.sizeDelta.y;

        private Sequence _openPopupSequence;
        private Sequence _closePopupSequence;

        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("PausePopupContinueButton").Button.onClick.AddListener(ClosePausePopup);
            UIManager.Instance.GetUI<UIButton>("PausePopupRetryButton").Button.onClick.AddListener(RetryBattle);
            UIManager.Instance.GetUI<UIButton>("PausePopupExitButton").Button.onClick.AddListener(ExitBattle);
        }

        public override void OpenPopup()
        {
            base.OpenPopup();

            popupBaseObj.transform.position = new Vector3(popupBaseObj.transform.position.x, -PopupBaseHeight);

            _openPopupSequence = DOTween.Sequence();
            _openPopupSequence.Append(backgroundImage.DOFade(shadowAlphaValue, backgroundShadowDuration));
            _openPopupSequence.Join(popupBaseObj.DOMoveY(0f, popupBaseTweenDuration));
        }

        public override void ClosePopup()
        {
            _closePopupSequence = DOTween.Sequence();
            _closePopupSequence.Append(backgroundImage.DOFade(0f, backgroundShadowDuration));
            _closePopupSequence.Join(popupBaseObj.DOMoveY(-PopupBaseHeight, popupBaseTweenDuration));
            _closePopupSequence.OnComplete(() => 
            {
                base.ClosePopup();
            });
        }
        
                
        private void ExitBattle()
        {
            GameManager.Instance.LoadScene(SceneType.Lobby);
        }

        private void RetryBattle()
        {
            GameManager.Instance.LoadScene(SceneType.Battle);
        }

        private void ClosePausePopup()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.ChangeEvent(GameStateType.Play);
            UIManager.Instance.GetUI<PausePopup>().ClosePopup();
        }
    }
}