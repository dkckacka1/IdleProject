using DG.Tweening;
using IdleProject.Core;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Battle.UI
{
    public class WinPopup : UIPopup
    {
        [SerializeField] private GameObject winTitle;
        [SerializeField] private GameObject reward;
        [SerializeField] private GameObject buttonsObject;

        [SerializeField] private ScrollRect rewardScroll;
        [SerializeField] private SlotUI slotUIPrefab;
        
        [BoxGroup("OpenTween"), SerializeField] private float openActiveInterval;
        [BoxGroup("OpenTween"), SerializeField] private float titleDuration;
        
        [BoxGroup("OpenTween/RewardTween"), SerializeField] private float createSlotInterval;
        [BoxGroup("OpenTween/RewardTween"), SerializeField] private float slotPunchScale;
        [BoxGroup("OpenTween/RewardTween"), SerializeField] private float slotPunchDuration;
        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("WinPopupNextStageButton").Button.onClick.AddListener(GotoNextStage);
            UIManager.Instance.GetUI<UIButton>("WinPopupRetryStageButton").Button.onClick.AddListener(RetryStage);
            UIManager.Instance.GetUI<UIButton>("WinPopupExitButton").Button.onClick.AddListener(GotoLobby);
        }

        public override void OpenPopup()
        {
            base.OpenPopup();

            var openSequence = DOTween.Sequence();
            openSequence.OnStart(() =>
            {
                reward.gameObject.SetActive(false);
                buttonsObject.gameObject.SetActive(false);
                
                ((RectTransform)winTitle.transform).anchoredPosition = new Vector3(0, 750, 0);
            });
            openSequence.Append(((RectTransform)winTitle.transform).DOAnchorPosY(400, titleDuration).SetEase(Ease.OutBack));
            openSequence.AppendInterval(openActiveInterval);
            openSequence.AppendCallback(() =>
            {
                reward.gameObject.SetActive(true);
            });
            openSequence.Append(SetRewardSequence(10));
            openSequence.AppendInterval(openActiveInterval);
            openSequence.AppendCallback(() =>
            {
                buttonsObject.gameObject.SetActive(true);
            });
        }

        private Sequence SetRewardSequence(int rewardCount)
        {
            var rewardSequence = DOTween.Sequence();

            for (int i = 0; i < rewardCount; ++i)
            {
                rewardSequence.AppendCallback(() =>
                {
                    var slot = CreateSlot();
                    ((RectTransform)slot.transform).DOPunchScale(Vector3.one * slotPunchScale,
                        slotPunchDuration, 1, 0.3f);
                });
                rewardSequence.AppendInterval(createSlotInterval);
            }

            return rewardSequence;
        }

        private SlotUI CreateSlot()
        {
            return Instantiate(slotUIPrefab, rewardScroll.content);
        }

        private void GotoNextStage()
        {
            Debug.Log($"다음 스테이지 시작");
        }

        private void RetryStage()
        {
            GameManager.Instance.LoadScene(SceneType.Battle);
        }

        private void GotoLobby()
        {
            GameManager.Instance.LoadScene(SceneType.Lobby);
        }
    }
}
