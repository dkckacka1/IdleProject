using System.Collections.Generic;
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
    // TODO : 보상 슬롯 UI 작업
    public class SelectStagePanel : UIPanel, IUISelectStageUpdatable
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

        private readonly List<SlotUI> _rewardSlotList;

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
            RectTransform.anchoredPosition = new Vector2(RectTransform.sizeDelta.x + 30f, RectTransform.anchoredPosition.y);
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
        
        private void GoToBattle()
        {
            if (_selectedStageData is null)
                return;

            DataManager.Instance.DataController.selectStaticStageData = _selectedStageData;
            GameManager.Instance.LoadScene(SceneType.Battle).Forget();
        }
    }
}