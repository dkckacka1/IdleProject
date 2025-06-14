using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace  IdleProject.Core.UI
{
    // 로딩 전용 UI 클래스
    public class LoadingUI : MonoBehaviour
    {
        [BoxGroup("LoadingUI"),SerializeField] private Canvas canvas;
        [BoxGroup("LoadingUI"), SerializeField] private CanvasGroup canvasGroup;
        [BoxGroup("LoadingUI"),SerializeField] private Slider loadingProgressBar;
        [BoxGroup("LoadingUI"),SerializeField] private TextMeshProUGUI loadingPercentText;

        [BoxGroup("LoadingTween"), SerializeField]
        private float fadeOutDuration = 1f;

        private void Awake()
        {
            loadingProgressBar.onValueChanged.AddListener(OnPercentChange);
        }

        public void LoadingStart()
        {
            canvasGroup.alpha = 1f;
            canvas.enabled = true;

            loadingProgressBar.maxValue = 1f;
            loadingProgressBar.value = 0f;
        }

        public void ShowLoadingPercent(float percent)
        {
            loadingProgressBar.value = percent;
        }

        public void LoadingEnd()
        {
            canvasGroup.DOFade(0f, fadeOutDuration).OnComplete(() => { canvas.enabled = false; });
        }


        private void OnPercentChange(float percent)
        {
            loadingPercentText.text = $"{percent * 100 :000} %";
        }
    }
}