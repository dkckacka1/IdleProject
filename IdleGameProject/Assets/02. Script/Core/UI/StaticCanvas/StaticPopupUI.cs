using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI
{
    public class StaticPopupUI : MonoBehaviour
    {
        [BoxGroup("StaticPopup"), SerializeField] protected Button closeButton;
        [BoxGroup("StaticPopup"), SerializeField] protected Button shadow;
        [BoxGroup("StaticPopup"), SerializeField] protected GameObject popup;
        
        [BoxGroup("Tween"), SerializeField] private float tweenDuration = 0.2f;
        [BoxGroup("Tween"), SerializeField] private Ease openTweenEase = Ease.OutBack;
        
        private Tween _openTween;

        protected virtual void Awake()
        {
            closeButton.onClick.AddListener(Close);
            shadow.onClick.AddListener(Close);
        }

        [BoxGroup("Tween"), Button]
        public virtual void Open()
        {
            gameObject.SetActive(true);
            popup.transform.localScale = Vector3.zero;
            _openTween = popup.transform.DOScale(Vector3.one, tweenDuration).SetEase(openTweenEase);
        }

        [BoxGroup("Tween"), Button]
        public virtual void Close()
        {
            gameObject.SetActive(false);

            if (_openTween is not null && _openTween.IsPlaying())
            {
                _openTween.Kill();
                _openTween = null;
            }
        }
    }
}