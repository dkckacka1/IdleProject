using System.Collections.Generic;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.StaticData;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class EquipmentItemSlotPanel : UIPanel
    {
        [SerializeField] private ScrollRect scroll;
        
        private readonly List<EquipmentItemSlot> _slotList = new List<EquipmentItemSlot>();
        
        public override void Initialized()
        {
        }

        public override void OpenPanel()
        {
            base.OpenPanel();

            var playerEquipmentItemList = DataManager.Instance.DataController.Player.PlayerData.userEquipmentItemList;

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
                    var data = playerEquipmentItemList[i].GetData;
                    var equipmentCharacterData = 
                        !string.IsNullOrEmpty(playerEquipmentItemList[i].equipmentCharacterName)
                        ? DataManager.Instance.GetData<StaticCharacterData>(playerEquipmentItemList[i].equipmentCharacterName)
                        : null;
                    
                    slot.SlotUI.SetData(data);
                    slot.SetIcon(equipmentCharacterData);
                    
                    // slot.PublishEvent<PointerEventData>(EventTriggerType.PointerClick, ClickCharacterSlot);
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
        }

        private EquipmentItemSlot CreateSlot()
        {
            return SlotUI.GetSlotUI<EquipmentItemSlot>(scroll.content);
        }
    }
}
