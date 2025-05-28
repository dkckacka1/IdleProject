using IdleProject.Core.ObjectPool;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace IdleProject.Battle.UI
{
    public class BattleText : MonoBehaviour, IPoolable
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
            BattleManager.Instance.GameStateEventBus.PublishEvent(GameStateType.Play, OnGamePlay);
            BattleManager.Instance.GameStateEventBus.PublishEvent(GameStateType.Pause, OnGamePause);
        }

        public void OnReleaseAction()
        {
            battleText.transform.position = Vector3.zero;
            floatingSequence = null;
            
            BattleManager.Instance.GameStateEventBus.RemoveEvent(GameStateType.Play, OnGamePlay);
            BattleManager.Instance.GameStateEventBus.RemoveEvent(GameStateType.Pause, OnGamePause);
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

        private void OnGamePause()
        {
            if (floatingSequence is not null)
            {
                floatingSequence.Pause();
            }
        }
        
        private void OnGamePlay()
        {
            if (floatingSequence is not null)
            {
                floatingSequence.Play();
            }
        }
    }
}