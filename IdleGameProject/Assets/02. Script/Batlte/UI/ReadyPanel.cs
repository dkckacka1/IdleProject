using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.Battle.UI
{
    public class ReadyPanel : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI readyText;

        [SerializeField] private float introDuration;
        [SerializeField] private float textChangeDuration;

        private Sequence _introSequence;

        private void Start()
        {
            transform.localPosition = new Vector3(-1920f, 0f, 0f);
        }

        public void PlayReadyUI(UnityAction onUIPlayEnd)
        {
            _introSequence = DOTween.Sequence();

            _introSequence.OnStart(() => { transform.localPosition = new Vector3(-1920f, 0f, 0f); });
            _introSequence.Append((transform as RectTransform).DOLocalMoveX(0f, introDuration));
            _introSequence.AppendInterval(textChangeDuration);
            _introSequence.AppendCallback(() => { readyText.text = "Fight"; });
            _introSequence.AppendInterval(textChangeDuration);
            _introSequence.Append((transform as RectTransform).DOLocalMoveX(1920f, introDuration));
            _introSequence.OnComplete(() => onUIPlayEnd?.Invoke());
        }
    }
}