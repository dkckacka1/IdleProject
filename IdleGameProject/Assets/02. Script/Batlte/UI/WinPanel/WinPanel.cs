using Cysharp.Threading.Tasks;
using DG.Tweening;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.StaticData;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Battle.UI
{
    public class WinPanel : UIPanel
    {
        [SerializeField] private RectTransform title;
        [SerializeField] private CanvasGroup reward;
        [SerializeField] private CanvasGroup buttonsObject;

        [SerializeField] private ScrollRect rewardScroll;
        [SerializeField] private SlotUI slotUIPrefab;
        
        [BoxGroup("OpenTween"), SerializeField] private float openActiveInterval;
        [BoxGroup("OpenTween"), SerializeField] private float titleDuration;
        
        [BoxGroup("OpenTween/RewardTween"), SerializeField] private float createSlotInterval;
        [BoxGroup("OpenTween/RewardTween"), SerializeField] private float slotPunchScale;
        [BoxGroup("OpenTween/RewardTween"), SerializeField] private float slotPunchDuration;
        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("WinPanelNextStageButton").Button.onClick.AddListener(GotoNextStage);
            UIManager.Instance.GetUI<UIButton>("WinPanelRetryStageButton").Button.onClick.AddListener(RetryStage);
            UIManager.Instance.GetUI<UIButton>("WinPanelExitButton").Button.onClick.AddListener(GotoLobby);
        }

        public override void OpenPanel()
        {
            base.OpenPanel();

            var openSequence = DOTween.Sequence();
            openSequence.OnStart(() =>
            {
                reward.gameObject.SetActive(false);
                buttonsObject.gameObject.SetActive(false);
                 
                title.anchoredPosition = new Vector3(0, 750, 0);
            });
            openSequence.Append(title.DOAnchorPosY(400, titleDuration).SetEase(Ease.OutBack));
            openSequence.AppendInterval(openActiveInterval);
            openSequence.AppendCallback(() =>
            {
                reward.gameObject.SetActive(true);
                reward.alpha = 0f;
                reward.DOFade(1f, 0.2f);
            });
            openSequence.Append(SetRewardSequence(10));
            openSequence.AppendInterval(openActiveInterval);
            openSequence.AppendCallback(() =>
            {
                buttonsObject.gameObject.SetActive(true);
                buttonsObject.alpha = 0f;
                buttonsObject.DOFade(1f, 0.2f);
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
            if (TryGetNextStage(DataManager.Instance.DataController.selectStaticStageData, out var nextStageData))
                // 다음 스테이지 있음
            {
                DataManager.Instance.DataController.selectStaticStageData = nextStageData;
                GameManager.Instance.LoadScene(SceneType.Battle).Forget();
            }
            else
                // 다음 스테이지 없음
            {
                // TODO
            }
        }

        private bool TryGetNextStage(StaticStageData currentStage, out StaticStageData nextStage)
        {
            nextStage = null;
            
            var nextStageIndex = currentStage.stageIndex + 1;
            var currentChapter = DataManager.Instance.GetData<StaticChapterData>(currentStage.chapterIndex.ToString());
            if (currentChapter.stageInfoList.Count >= nextStageIndex)
                // 현재 챕터에 다음 스테이지 있음
            {
                nextStage = DataManager.Instance.GetData<StaticStageData>($"{currentChapter.chapterIndex}-{nextStageIndex}");
            }
            else
                // 다음 챕터 확인
            {
                var nextChapterIndex = currentChapter.chapterIndex + 1;
                if (DataManager.Instance.TryGetData(nextChapterIndex.ToString(), out StaticChapterData nextChapter))
                {
                    nextStage = DataManager.Instance.GetData<StaticStageData>($"{nextChapter.chapterIndex}-{1}");
                }
            }

            return nextStage is not null;
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
