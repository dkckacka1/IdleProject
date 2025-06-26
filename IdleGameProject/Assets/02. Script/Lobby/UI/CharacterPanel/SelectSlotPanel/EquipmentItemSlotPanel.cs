using System.Collections.Generic;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class EquipmentItemSlotPanel : UIPanel
    {
        [SerializeField] private ScrollRect scroll;
        
        private readonly List<SlotUI> _slotList = new();
        
        private SlotUI _selectSlot;
        
        public override void Initialized()
        {
        }

        public override void OpenPanel()
        {
            base.OpenPanel();

            var playerEquipmentItemList =
                DataManager.Instance.DataController.Player.GetEquipmentItemList;

            int createCount =  playerEquipmentItemList.Count - _slotList.Count;
            
            for (int i = 0; i < createCount; ++i)
            {
                var slot = CreateSlot();
                _slotList.Add(slot);
            }

            for (int i = 0; i < playerEquipmentItemList.Count; ++i)
            {
                var slot = _slotList[i];
                if (i <= playerEquipmentItemList.Count - 1)
                {
                    var data = playerEquipmentItemList[i].GetData<DynamicEquipmentItemData>();
                    var equipmentCharacterData = 
                        !string.IsNullOrEmpty(playerEquipmentItemList[i].equipmentCharacterName)
                        ? DataManager.Instance.GetData<StaticCharacterData>(playerEquipmentItemList[i].equipmentCharacterName)
                        : null;
                    
                    slot.SetData(data);
                    slot.GetSlotParts<EquipmentItemSlot>().SetEquipmentCharacterIcon(equipmentCharacterData);
                    
                    slot.PublishEvent<PointerEventData>(EventTriggerType.PointerClick, ClickEquipmentSlot);
                    slot.gameObject.SetActive(true);
                }
                else
                {
                    slot.UnPublishAllEvent();
                    slot.gameObject.SetActive(false);
                }
            }
        }

        private void ClickEquipmentSlot(PointerEventData eventData, SlotUI slot)
        {
            SwapSlotFocus(slot);
            var equipmentItemData = slot.GetData<DynamicEquipmentItemData>();

            foreach (var selectEquipmentItemUpdatable in UIManager.Instance.GetUIsOfType<IUISelectEquipmentItemUpdatable>())
            {
                selectEquipmentItemUpdatable.SelectEquipmentItem(equipmentItemData);
            }
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
            return SlotUI.GetSlotUI<EquipmentItemSlot>(scroll.content);
        }
    }
}
