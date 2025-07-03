using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.StaticData;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Lobby.UI.StagePanel
{
    public class SelectStagePanel : UIPanel, IUISelectStageUpdatable, IUISelectChapterUpdatable
    {
        [BoxGroup("StagePanel"), SerializeField]
        private TextMeshProUGUI stageNameText;

        [BoxGroup("StagePanel"), SerializeField]
        private Transform rewardSlotParent;

        [BoxGroup("StagePanel"), SerializeField]
        private List<CharacterSlot> enemySlotList;

        [BoxGroup("StagePanel/OpenTween"), SerializeField]
        private float openTweenDuration;

        [BoxGroup("StagePanel/OpenTween"), SerializeField]
        private Ease openEase;

        [BoxGroup("StagePanel/OpenTween"), SerializeField]
        private float overShoot;

        private readonly List<SlotUI> _rewardSlotList = new List<SlotUI>();

        private Tween _openTween;

        private StaticStageData _selectedStageData;

        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("GotoBattleButton").Button.onClick.AddListener(GoToBattle);
            UIManager.Instance.GetUI<UIButton>("StagePanelExitButton").Button.onClick.AddListener(ClosePanel);
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            RectTransform.anchoredPosition =
                new Vector2(RectTransform.sizeDelta.x + 30f, RectTransform.anchoredPosition.y);
            _openTween = RectTransform.DOAnchorPosX(-30f, openTweenDuration).SetEase(openEase, overshoot: overShoot);
        }

        public override void ClosePanel()
        {
            if (_openTween is not null && _openTween.IsPlaying())
            {
                _openTween.Kill();
            }

            RectTransform.anchoredPosition = new Vector2(-30f, RectTransform.anchoredPosition.y);
            RectTransform.DOAnchorPosX(RectTransform.sizeDelta.x + 30f, openTweenDuration)
                .SetEase(openEase, overshoot: overShoot).OnComplete(() => { base.ClosePanel(); });
        }

        public void SelectStageUpdatable(StaticStageData selectStage)
        {
            OpenPanel();
            SetEnemyList(selectStage);
        }

        private void SetEnemyList(StaticStageData selectStage)
        {
            _selectedStageData = selectStage;

            stageNameText.text =
                $"{selectStage.stageName} <size=75%><color=#AAAAAA>챕터 {selectStage.chapterIndex}</color></size>";

            SetEnemy(enemySlotList[0], selectStage.stageFormation.frontLeftPositionInfo);
            SetEnemy(enemySlotList[1], selectStage.stageFormation.frontMiddlePositionInfo);
            SetEnemy(enemySlotList[2], selectStage.stageFormation.frontRightPositionInfo);
            SetEnemy(enemySlotList[3], selectStage.stageFormation.rearLeftPositionInfo);
            SetEnemy(enemySlotList[4], selectStage.stageFormation.rearRightPositionInfo);

            SetReward(selectStage.rewardList);
        }

        private void SetEnemy(CharacterSlot enemySlot, PositionInfo positionInfo)
        {
            if (!string.IsNullOrEmpty(positionInfo.characterName))
            {
                var characterData = DataManager.Instance.GetData<StaticCharacterData>(positionInfo.characterName);
                enemySlot.SlotUI.BindData(characterData);
                enemySlot.SetLevel(positionInfo.characterLevel);
                enemySlot.gameObject.SetActive(true);
            }
            else
            {
                enemySlot.gameObject.SetActive(false);
            }
        }

        private void SetReward(List<RewardInfo> rewardList)
        {
            CreateRewardSlot(rewardList);
            BindRewardData(rewardList);
        }

        private void BindRewardData(List<RewardInfo> rewardList)
        {
            // Consumable 처리
            var consumableSlots = _rewardSlotList
                .Where(slot => slot.hasSlotParts<ConsumableItemSlot>())
                .Select(slot => slot.GetSlotParts<ConsumableItemSlot>())
                .ToList();

            var consumableRewards = rewardList
                .Where(info => info.rewardType == RewardType.ConsumableItem)
                .ToList();

            for (int i = 0; i < consumableSlots.Count; ++i)
            {
                if (i < consumableRewards.Count)
                {
                    var reward = consumableRewards[i];
                    var data = DataManager.Instance.GetData<StaticConsumableItemData>(reward.itemIndex);

                    consumableSlots[i].SlotUI.BindData(data);
                    consumableSlots[i].SetCount(reward.count);
                    consumableSlots[i].SetShadow(false);
                    consumableSlots[i].gameObject.SetActive(true);
                }
                else
                {
                    consumableSlots[i].gameObject.SetActive(false);
                }
            }

            // Equipment 처리
            var equipmentSlots = _rewardSlotList
                .Where(slot => slot.hasSlotParts<EquipmentItemSlot>())
                .Select(slot => slot.GetSlotParts<EquipmentItemSlot>())
                .ToList();

            var equipmentRewards = rewardList
                .Where(info => info.rewardType == RewardType.EquipmentItem)
                .ToList();

            for (int i = 0; i < equipmentSlots.Count; ++i)
            {
                if (i < equipmentRewards.Count)
                {
                    var reward = equipmentRewards[i];
                    var data = DataManager.Instance.GetData<StaticEquipmentItemData>(reward.itemIndex);

                    equipmentSlots[i].SlotUI.BindData(data);
                    equipmentSlots[i].SetEquipmentObject(false);
                    equipmentSlots[i].gameObject.SetActive(true);
                }
                else
                {
                    equipmentSlots[i].gameObject.SetActive(false);
                }
            }
        }

        private void CreateRewardSlot(List<RewardInfo> rewardList)
        {
            // 슬롯 생성
            var consumableItemSlotCount = _rewardSlotList.Count(slot => slot.hasSlotParts<ConsumableItemSlot>());
            var consumableItemRewardCount = rewardList.Count(reward => reward.rewardType == RewardType.ConsumableItem);
            CreateSlots<ConsumableItemSlot>(consumableItemSlotCount, consumableItemRewardCount);

            var equipmentItemSlotCount = _rewardSlotList.Count(slot => slot.hasSlotParts<EquipmentItemSlot>());
            var equipmentItemRewardCount = rewardList.Count(info => info.rewardType == RewardType.EquipmentItem);
            CreateSlots<EquipmentItemSlot>(equipmentItemSlotCount, equipmentItemRewardCount);

            // 슬롯 정렬
            _rewardSlotList.Sort((lhs, rhs) =>
            {
                int GetSlotTypeOrder(SlotUI slot)
                {
                    if (slot.hasSlotParts<ConsumableItemSlot>()) return 0;
                    if (slot.hasSlotParts<EquipmentItemSlot>()) return 1;
                    return int.MaxValue; // 기타 슬롯은 뒤로
                }

                return GetSlotTypeOrder(lhs).CompareTo(GetSlotTypeOrder(rhs));
            });

            // 정렬된 순서대로 transform 순서도 적용
            for (int i = 0; i < _rewardSlotList.Count; ++i)
            {
                _rewardSlotList[i].transform.SetSiblingIndex(i);
            }
        }

        private void CreateSlots<T>(int slotCount, int rewardCount) where T : SlotParts
        {
            var createSlotCount = rewardCount - slotCount;

            for (int i = 0; i < createSlotCount; ++i)
            {
                _rewardSlotList.Add(SlotUI.GetSlotUI<T>(rewardSlotParent));
            }
        }

        private void GoToBattle()
        {
            if (_selectedStageData is null)
                return;

            DataManager.Instance.DataController.selectStaticStageData = _selectedStageData;
            GameManager.Instance.LoadScene(SceneType.Battle).Forget();
        }

        public void SelectChapterUpdatable(StaticChapterData selectChapter)
        {
            if (_selectedStageData is not null)
            {
                ClosePanel();
            }
        }
    }
}