using System;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Engine.Core.EventBus;

namespace IdleProject.Battle.UI
{
    public class BattleText : MonoBehaviour, IPoolable, IEnumEvent<GameStateType>
    {
        private TextMeshProUGUI battleText;

        [SerializeField] private float punchDuration = 0.2f;
        [SerializeField] private Vector3 punchScaleVector = new Vector3(1.1f, 1.1f);

        [SerializeField] private float floatingDuration = 0.5f;
        [SerializeField] private float floatingYPos = 100f;

        private Sequence floatingSequence;
        
        private void Awake()
        {
            battleText = GetComponent<TextMeshProUGUI>();

            floatingSequence = DOTween.Sequence();
        }

        public void OnCreateAction()
        {
        }

        public void OnGetAction()
        {
            BattleManager.Instance.GameStateEventBus.PublishEvent(this);
        }

        public void OnReleaseAction()
        {
            battleText.transform.position = Vector3.zero;
            floatingSequence = null;
            
            BattleManager.Instance.GameStateEventBus.RemoveEvent(this);
        }

        public void ShowText(Vector3 textPosition, string text)
        {
            transform.position = textPosition;
            battleText.text = text;
            SetSequence();
        }

        private void SetSequence()
        {
            floatingSequence = DOTween.Sequence();
            
            floatingSequence.Append(battleText.transform.DOPunchScale(punchScaleVector,
                punchDuration / BattleManager.GetCurrentBattleSpeed));
            floatingSequence.Join(battleText.transform.DOMoveY(transform.position.y + floatingYPos,
                floatingDuration / BattleManager.GetCurrentBattleSpeed));

            floatingSequence.OnComplete(() =>
            {
                ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
            });
        }

        public void OnEnumChange(GameStateType type)
        {
            switch (type)
            {
                case GameStateType.Play:
                    floatingSequence?.Play();
                    break;
                case GameStateType.Pause:
                    floatingSequence?.Pause();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}