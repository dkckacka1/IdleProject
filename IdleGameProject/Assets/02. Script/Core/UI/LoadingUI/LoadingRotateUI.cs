using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace IdleProject.Core.UI.Loading
{
    public class LoadingRotateUI : UIBase
    {
        [SerializeField] private float rotateDuration = 1f;
        
        private Tween _rotateTween;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public async UniTask StartLoading(UniTask loadingTask)
        {
            gameObject.SetActive(true);
            _rotateTween = RectTransform.DORotate(new Vector3(0, 0, -360f), rotateDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .OnKill(() => { RectTransform.Rotate(Vector3.zero); });

            await loadingTask;
            
            _rotateTween.Kill();
            gameObject.SetActive(false);
        }
    }
}