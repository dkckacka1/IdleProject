using System;
using DG.Tweening;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI
{
    public class ConfirmPopup : MonoBehaviour
    {
        [SerializeField] private GameObject popup;
        [SerializeField] private TextMeshProUGUI descriptionText;
        [SerializeField] private Button yesButton;
        [SerializeField] private Button noButton;
        [SerializeField] private Button shadow;

        [BoxGroup("Tween"), SerializeField] private float tweenDuration;

        [BoxGroup("Tween"), SerializeField] private Ease openTweenEase;


        private Tween _openTween;

        private void Awake()
        {
            noButton.onClick.AddListener(Close);
            shadow.onClick.AddListener(Close);
        }

        [BoxGroup("Tween"), Button]
        public void Open()
        {
            gameObject.SetActive(true);
            popup.transform.localScale = Vector3.zero;
            popup.transform.DOScale(Vector3.one, tweenDuration).SetEase(openTweenEase);
        }

        [BoxGroup("Tween"), Button]
        public void Close()
        {
            yesButton.onClick.RemoveAllListeners();
            gameObject.SetActive(false);

            if (_openTween is not null && _openTween.IsPlaying())
            {
                _openTween.Kill();
                _openTween = null;
            }
        }

        public void SetConfirm(string description, UnityAction yesAction)
        {
            descriptionText.text = description;
            yesButton.onClick.AddListener(yesAction);
            yesButton.onClick.AddListener(Close);
        }
    }
}