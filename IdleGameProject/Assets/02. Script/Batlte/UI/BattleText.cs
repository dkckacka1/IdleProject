using System;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Engine.Core.EventBus;
using IdleProject.Core;

namespace IdleProject.Battle.UI
{
    public class BattleText : MonoBehaviour, IPoolable, IEnumEvent<GameStateType>
    {
        private TextMeshProUGUI _battleText;

        [SerializeField] private float punchDuration = 0.2f;
        [SerializeField] private Vector3 punchScaleVector = new Vector3(1.1f, 1.1f);

        [SerializeField] private float floatingDuration = 0.5f;
        [SerializeField] private float floatingYPos = 100f;

        private Sequence _floatingSequence;
        
        private void Awake()
        {
            _battleText = GetComponent<TextMeshProUGUI>();

            _floatingSequence = DOTween.Sequence();
        }

        public void OnCreateAction()
        {
        }

        public void OnGetAction()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.PublishEvent(this);
        }

        public void OnReleaseAction()
        {
            _battleText.transform.position = Vector3.zero;
            _floatingSequence = null;
            
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.RemoveEvent(this);
        }

        public void ShowText(Vector3 textPosition, string text)
        {
            transform.position = textPosition;
            _battleText.text = text;
            SetSequence();
        }

        private void SetSequence()
        {
            _floatingSequence = DOTween.Sequence();
            
            _floatingSequence.Append(_battleText.transform.DOPunchScale(punchScaleVector,
                punchDuration / BattleManager.GetCurrentBattleSpeed));
            _floatingSequence.Join(_battleText.transform.DOMoveY(transform.position.y + floatingYPos,
                floatingDuration / BattleManager.GetCurrentBattleSpeed));

            _floatingSequence.OnComplete(() =>
            {
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            });
        }

        public void OnEnumChange(GameStateType type)
        {
            switch (type)
            {
                case GameStateType.Play:
                    _floatingSequence?.Play();
                    break;
                case GameStateType.Pause:
                    _floatingSequence?.Pause();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}