using IdleProject.Core.ObjectPool;
using UnityEngine;
using TMPro;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Engine.Core.Time;

namespace IdleProject.Battle.UI
{
    public class BattleText : MonoBehaviour, IPoolable
    {
        private TextMeshProUGUI battleText;

        [SerializeField] private float punchDuration = 0.2f;
        [SerializeField] private Vector3 punchScaleVector = new Vector3(1.1f, 1.1f);

        [SerializeField] private float floatingDuration = 0.5f;
        [SerializeField] private float floatingYPos = 100f;



        private void Awake()
        {
            battleText = GetComponent<TextMeshProUGUI>();
        }

        public void OnCreateAction()
        {
        }

        public void OnGetAction()
        {

        }

        public void OnReleaseAction()
        {
            battleText.transform.position = Vector3.zero;
        }

        public async UniTaskVoid ShowText(Vector3 textPosition, string text)
        {
            transform.position = textPosition;
            battleText.text = text;
            battleText.transform.DOPunchScale(punchScaleVector, punchDuration / BattleManager.GetCurrentBattleSpeed);
            battleText.transform.DOMoveY(transform.position.y + floatingYPos, floatingDuration  / BattleManager.GetCurrentBattleSpeed);
            await BattleManager.GetBattleTimer(floatingDuration);
            ObjectPoolManager.Instance.Release(GetComponent<PoolableObject>());
        }
    }
}