using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Data.StaticData;
#if UNITY_EDITOR
using IdleProject.EditorClass;
#endif
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.StagePanel
{
    public class ShowStagePanel : UIPanel, IUISelectChapterUpdatable
    {
        public StaticChapterData SelectedChapter { get; private set; }

        [SerializeField] private Image backgroundImage;
        [SerializeField] private Transform stageSlotParent;

        private UIDropdown _chapterDropdown;

        private readonly List<StageSlot> _stageSlotList = new List<StageSlot>();


        public override void Initialized()
        {
            _chapterDropdown = UIManager.Instance.GetUI<UIDropdown>("ChapterDropdown");

            var chapterList = DataManager.Instance.GetDataList<StaticChapterData>();

            _chapterDropdown.Dropdown.AddOptions(chapterList
                .Select(chapter => $"<size=75%>Chapter</size> {chapter.chapterIndex}").ToList());
            _chapterDropdown.Dropdown.onValueChanged.AddListener(OnDropdownChanged);
        }

        public override void OpenPanel()
        {
            base.OpenPanel();

            if (SelectedChapter is null)
            {
                SelectChapter(DataManager.Instance.GetData<StaticChapterData>("1"));
            }
        }

        private void OnDropdownChanged(int chapterIndex)
        {
            var selectIndex = chapterIndex + 1;
            var chapterData = DataManager.Instance.GetData<StaticChapterData>($"{selectIndex}");

            SelectChapter(chapterData);
        }

        public void SelectChapter(StaticChapterData chapterData)
        {
            SelectedChapter = chapterData;
            foreach (var updatable in UIManager.Instance.GetUIsOfType<IUISelectChapterUpdatable>())
            {
                updatable.SelectChapterUpdatable(chapterData);
            }
        }

        private void BindStageData()
        {
            var lastStage = DataManager.Instance.DataController.Player.GetLastStage();
            for (int i = 0; i < _stageSlotList.Count; ++i)
            {
                var stageSlot = _stageSlotList[i];
                if (i <= SelectedChapter.stageInfoList.Count - 1)
                {
                    var stageInfo = SelectedChapter.stageInfoList[i];
                    var stageName = $"{SelectedChapter.chapterIndex}-{stageInfo.stageIndex}";
                    var stageData = DataManager.Instance.GetData<StaticStageData>(stageName);
                    var isClear = DataManager.Instance.DataController.Player.PlayerClearStageSet.Contains(stageName);
                    var isLastStage = lastStage == stageData;

                    stageSlot.SetStage(stageData, isClear, isLastStage);
                    ((RectTransform)stageSlot.transform).anchoredPosition = new Vector2(stageInfo.posX, stageInfo.posY);
                }
                else
                {
                    _stageSlotList[i].gameObject.SetActive(false);
                }
            }
        }

        private void CreateStageSlots(StaticChapterData selectChapterData)
        {
            var createCount = selectChapterData.stageInfoList.Count - _stageSlotList.Count;

            for (int i = 0; i < createCount; ++i)
            {
                CreateStageSlot();
            }
        }

        private void CreateStageSlot()
        {
            var slotObject = ResourceManager.Instance.GetPrefab(ResourceManager.UIPrefab, nameof(StageSlot));
            var slotInstance = Instantiate(slotObject, stageSlotParent).GetComponent<StageSlot>();
            _stageSlotList.Add(slotInstance);
        }


        public void SelectChapterUpdatable(StaticChapterData selectChapter)
        {
            CreateStageSlots(SelectedChapter);
            BindStageData();
        }
#if UNITY_EDITOR
        #region Creator

        [BoxGroup("Creator"), Button]
        private void CreateChapterData()
        {
            StaticDataCreator.CreateStaticData<StaticChapterData>("ChapterData", chapterData =>
            {
                chapterData.chapterImage = backgroundImage.sprite.name;

                var stageIndex = 0;

                foreach (var slot in GetComponentsInChildren<StageSlot>(true))
                {
                    chapterData.stageInfoList.Add(new StageInfo
                    {
                        stageIndex = int.Parse(slot.gameObject.name),
                        posX = ((RectTransform)slot.transform).anchoredPosition.x,
                        posY = ((RectTransform)slot.transform).anchoredPosition.y
                    });

                    ++stageIndex;
                    var index = stageIndex;
                    StaticDataCreator.CreateStaticData<StaticStageData>($"StageData {stageIndex}",
                        stageData => { stageData.stageIndex = index; });
                }
            });
        }

        [BoxGroup("Creator"), SerializeField] private StageSlot slot;

        [BoxGroup("Creator"), Button]
        private void CreateSlot()
        {
            var slotInstance = Instantiate(slot, stageSlotParent).GetComponent<StageSlot>();
            _stageSlotList.Add(slotInstance);
            slotInstance.gameObject.name = _stageSlotList.Count.ToString();
        }

        [BoxGroup("Creator"), Button]
        private void DestroySlots()
        {
            foreach (var slot in _stageSlotList)
            {
                Destroy(slot.gameObject);
            }
        }

        #endregion
#endif
    }
}