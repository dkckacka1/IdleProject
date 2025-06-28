using System.Collections.Generic;
using System.Linq;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.CharacterPanel
{
    public class CharacterSlotPanel : UIPanel
    {
        [SerializeField] private ScrollRect slotScrollRect;
        
        private readonly List<SlotUI> _slotList = new List<SlotUI>();
        
        private SlotUI _selectSlot;
        
        public override void Initialized()
        {
            var userCharacterList = DataManager.Instance.DataController.Player.PlayerCharacterDataDic.Values.ToList();

            CreateSlot(userCharacterList);
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            var userCharacterList = DataManager.Instance.DataController.Player.PlayerCharacterDataDic.Values.ToList();
            var selectCharacter = UIManager.Instance.GetUI<SelectCharacterPanel>().SelectedCharacter;
            CreateSlot(userCharacterList);
            for (int i = 0; i < _slotList.Count; ++i)  
                // 캐릭터 수만큼 슬롯에 정의
            {
                var slot = _slotList[i];
                if (i <= userCharacterList.Count - 1)
                {
                    slot.BindData(userCharacterList[i]);
                    slot.PublishEvent<PointerEventData>(EventTriggerType.PointerClick, ClickCharacterSlot);
                    if(selectCharacter == userCharacterList[i])
                        SwapSlotFocus(slot);
                    slot.gameObject.SetActive(true);
                }
                else
                {
                    slot.gameObject.SetActive(false);
                }
            }
        }


        public override void ClosePanel()
        {
            base.ClosePanel();
            foreach (var slot in _slotList)
            {
                slot.UnPublishAllEvent();
                slot.gameObject.SetActive(false);
            }
        }
        
        private void CreateSlot(List<DynamicCharacterData> userCharacterList)
        {
            var createSlotCount = userCharacterList.Count - _slotList.Count;
            for (int i = 0; i < createSlotCount; ++i)
                // 부족한 슬롯 생성
            {
                _slotList.Add(CreateSlot());
            }
        }

        private void ClickCharacterSlot(PointerEventData eventData, SlotUI slot)
        {
            SwapSlotFocus(slot);
            var characterData = slot.GetData<DynamicCharacterData>();

            UIManager.Instance.GetUI<SelectCharacterPanel>().SelectCharacter(characterData);
        }

        private void SwapSlotFocus(SlotUI slot)
        {
            if (_selectSlot is not null)
                _selectSlot.SetFocus(false);

            _selectSlot = slot;
            _selectSlot.SetFocus(true);
        }

        private SlotUI CreateSlot()
        {
            return SlotUI.GetSlotUI<CharacterSlot>(slotScrollRect.content);
        }
    }
}
