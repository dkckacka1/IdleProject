using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Data.StaticData;
using IdleProject.EditorClass;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.StagePanel
{
    public class ShowStagePanel : UIPanel
    {
        [SerializeField] private Image backgroundImage;
        [SerializeField] private Transform stageSlotParent;

        private readonly List<StageSlot> _stageSlotList = new List<StageSlot>();
        
        public override void Initialized()
        {
            // StaticChapterData selectChapterData = null;
            //
            // CreateStageSlots(selectChapterData);
            //
            // for (int i = 0; i < _stageSlotList.Count; ++i)
            // {
            //     if (i <= selectChapterData.stageInfoList.Count - 1)
            //     {
            //         var stageSlot = _stageSlotList[i];
            //         var stageInfo = selectChapterData.stageInfoList[i];
            //         var stageName = $"{selectChapterData.chapterIndex}-{stageInfo.stageIndex}";
            //         var stageData = DataManager.Instance.GetData<StaticStageData>(stageName);
            //         var isClear = DataManager.Instance.DataController.Player.PlayerClearStageList.Any(clearStage => clearStage == stageName);
            //         
            //         stageSlot.SetStage(stageData, isClear);
            //         // TODO
            //         // stageSlot.button.onClick.AddListener(GoStage(stageData));
            //     }
            //     else
            //     {
            //         _stageSlotList[i].gameObject.SetActive(false);
            //     }
            // }
        }

        private void CreateStageSlots(StaticChapterData selectChapterData)
        {
            var createCount = selectChapterData.stageInfoList.Count - _stageSlotList.Count;

            for (int i = 0; i < createCount; ++i)
            {
                CreateStageSlot();
            }
        }

        private StageSlot CreateStageSlot()
        {
            var slotObject = ResourceManager.Instance.GetPrefab(ResourceManager.UIPrefab, nameof(StageSlot));
            var slotInstance = Instantiate(slotObject, stageSlotParent).GetComponent<StageSlot>();
            _stageSlotList.Add(slotInstance);
            
            return slotInstance;
        }
        

        [Button("CreateChapter")]
        private void CreateChapterData()
        {
            StaticDataCreator.CreateStaticData<StaticChapterData>("ChapterData", asset =>
            {
                asset.chapterImage = backgroundImage.sprite.name;

                foreach (var slot in GetComponentsInChildren<StageSlot>(true))
                {
                    asset.stageInfoList.Add(new StageInfo
                    {
                        stageIndex = int.Parse(slot.gameObject.name),
                        posX = ((RectTransform)slot.transform).anchoredPosition.x,
                        posY = ((RectTransform)slot.transform).anchoredPosition.y
                    });
                }
            });
        }
    }
}