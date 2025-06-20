using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace IdleProject.Core.UI.Loading
{
    public class LoadingRotateUI : MonoBehaviour
    {
        [SerializeField] private float rotateDuration = 1f;
        
        private Tween _rotateTween;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public async UniTaskVoid StartLoading(UniTask loadingTask)
        {
            gameObject.SetActive(true);
            _rotateTween = transform.DORotate(new Vector3(0, 360f, 0), rotateDuration, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Restart)
                .OnKill(() => { transform.Rotate(Vector3.zero); });

            await loadingTask;
            
            _rotateTween.Kill();
            gameObject.SetActive(false);
        }
    }
}