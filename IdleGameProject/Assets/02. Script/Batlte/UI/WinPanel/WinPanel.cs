using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Sound;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.SerializableData;
using IdleProject.Data.StaticData;
using IdleProject.Util;
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

        [SerializeField] private string getItemSoundName;
        
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
            openSequence.Append(SetRewardSequence());
            openSequence.AppendInterval(openActiveInterval);
            openSequence.AppendCallback(() =>
            {
                buttonsObject.gameObject.SetActive(true);
                buttonsObject.alpha = 0f;
                buttonsObject.DOFade(1f, 0.2f);
            });
        }

        private Sequence SetRewardSequence()
        {
            var rewardSequence = DOTween.Sequence();
            var rewardList = DataManager.Instance.DataController.selectStaticStageData.rewardDataList;
            
            for (int i = 0; i < rewardList.Count; ++i)
            {
                var rewardInfo = rewardList[i];
                rewardSequence.AppendCallback(() =>
                {
                    var slot = CreateSlot(rewardInfo);
                    ((RectTransform)slot.transform).DOPunchScale(Vector3.one * slotPunchScale,
                        slotPunchDuration, 1, 0.3f);
                    SoundManager.Instance.PlaySfx(getItemSoundName);
                });
                rewardSequence.AppendInterval(createSlotInterval);
            }

            return rewardSequence;
        }

        private SlotUI CreateSlot(RewardData rewardData)
        {
            SlotUI slot = null;

            switch (rewardData.itemData)
            {
                case StaticConsumableItemData consumableItem:
                    {
                        slot = SlotUI.GetSlotUI<ConsumableItemSlot>(rewardScroll.content);
                        slot.BindData(consumableItem);
                        slot.GetSlotParts<ConsumableItemSlot>().SetCount(rewardData.count);
                        slot.GetSlotParts<ConsumableItemSlot>().SetShadow(false);
                    }
                    break;
                case StaticEquipmentItemData equipmentItem:
                    {
                        slot = SlotUI.GetSlotUI<EquipmentItemSlot>(rewardScroll.content);
                        slot.BindData(equipmentItem);
                        slot.GetSlotParts<EquipmentItemSlot>().SetEquipmentObject(false);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            return slot;
        }

        private void GotoNextStage()
        {
            if (DataExtension.TryGetNextStage(DataManager.Instance.DataController.selectStaticStageData, out var nextStageData))
                // 다음 스테이지 있음
            {
                DataManager.Instance.DataController.selectStaticStageData = nextStageData;
                GameManager.Instance.LoadScene(SceneType.Battle).Forget();
            }
            else
                // 다음 스테이지 없음
            {
                UIManager.Instance.OpenToastPopup("다음 스테이지가 없습니다. 업데이트를 기다려 주세요.");
            }
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
