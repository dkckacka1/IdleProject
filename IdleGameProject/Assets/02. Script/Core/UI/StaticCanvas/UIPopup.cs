using DG.Tweening;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI
{
    public class UIPopup : UIBase, IUIInit
    {
        [BoxGroup("Popup"), SerializeField] protected Button closeButton;
        [BoxGroup("Popup"), SerializeField] protected Button shadow;
        [BoxGroup("Popup"), SerializeField] protected GameObject popup;
        
        [BoxGroup("Tween"), SerializeField] private float tweenDuration = 0.2f;
        [BoxGroup("Tween"), SerializeField] private Ease openTweenEase = Ease.OutBack;
        
        private Tween _openTween;

        protected override void Awake()
        {
            base.Awake();
            
            closeButton.onClick.AddListener(ClosePopup);
            shadow.onClick.AddListener(ClosePopup);
        }

        [BoxGroup("Tween"), Button]
        public virtual void OpenPopup()
        {
            gameObject.SetActive(true);
            popup.transform.localScale = Vector3.zero;
            _openTween = popup.transform.DOScale(Vector3.one, tweenDuration).SetEase(openTweenEase);
        }

        [BoxGroup("Tween"), Button]
        public virtual void ClosePopup()
        {
            gameObject.SetActive(false);

            if (_openTween is not null && _openTween.IsPlaying())
            {
                _openTween.Kill();
                _openTween = null;
            }
        }

        public virtual void Initialized() {}
    }
}