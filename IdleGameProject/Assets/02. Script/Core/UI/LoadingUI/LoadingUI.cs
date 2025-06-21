using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace  IdleProject.Core.UI.Loading
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

        [SerializeField] private float progressBarGageFillDuration = 0.2f;

        private float _currentLoadingValue;
        private float _fillAmount;
        
        public bool isLoading;

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
            _currentLoadingValue = 0f;
            isLoading = true;
            
            FillLoadingGage().Forget();
        }
        
        public void ShowLoadingPercent(float percent)
        {
            _currentLoadingValue = percent;
            _fillAmount = _currentLoadingValue - loadingProgressBar.value;
        }

        private void OnPercentChange(float percent)
        {
            loadingPercentText.text = $"{percent * 100 :0} %";
        }

        private async UniTaskVoid FillLoadingGage()
        {
            while (loadingProgressBar.value < 1)
            {
                if (loadingProgressBar.value < _currentLoadingValue)
                {
                    loadingProgressBar.value += _fillAmount * Time.deltaTime / progressBarGageFillDuration;
                    if (loadingProgressBar.value > _currentLoadingValue)
                        loadingProgressBar.value = _currentLoadingValue;
                }

                await UniTask.WaitForEndOfFrame();
            }

            LoadingEnd();
        }
        
        private void LoadingEnd()
        {
            canvasGroup.DOFade(0f, fadeOutDuration).OnComplete(() =>
            {
                isLoading = false;
                canvas.enabled = false;
            });
        }

    }
}