using System.Collections.Generic;
using System.Linq;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.CharacterPanel
{
    public class EquipmentItemSlotPanel : UIPanel, IUIUpdatable, IUISelectEquipmentItemUpdatable
    {
        [SerializeField] private ScrollRect scroll;

        private readonly List<SlotUI> _slotList = new();

        private SlotUI _selectSlot;

        public override void Initialized()
        {
            var playerEquipmentItemList =
                DataManager.Instance.DataController.Player.PlayerEquipmentItemDataDic.Values.ToList();

            CreateSlots(playerEquipmentItemList);
        }

        public override void OpenPanel()
        {
            base.OpenPanel();

            var playerEquipmentItemList =
                DataManager.Instance.DataController.Player.PlayerEquipmentItemDataDic.Values.ToList();

            CreateSlots(playerEquipmentItemList);

            for (int i = 0; i < playerEquipmentItemList.Count; ++i)
            {
                var slot = _slotList[i];
                if (i <= playerEquipmentItemList.Count - 1)
                {
                    var data = playerEquipmentItemList[i].GetData<DynamicEquipmentItemData>();
                    slot.BindData(data);

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

        private void CreateSlots(List<DynamicEquipmentItemData> playerEquipmentItemList)
        {
            int createCount = playerEquipmentItemList.Count - _slotList.Count;

            for (int i = 0; i < createCount; ++i)
            {
                var slot = CreateSlot();
                _slotList.Add(slot);
            }
        }

        private void ClickEquipmentSlot(PointerEventData eventData, SlotUI slot)
        {
            SwapFocusSlot(slot);
            var equipmentItemData = slot.GetData<DynamicEquipmentItemData>();

            UIManager.Instance.GetUI<SelectCharacterPanel>().SelectEquipmentItem(equipmentItemData);
        }

        private void SwapFocusSlot(SlotUI slot)
        {
            if (_selectSlot == slot)
                return;

            if (_selectSlot is not null)
                _selectSlot.SetFocus(false);

            _selectSlot = slot;
            _selectSlot.SetFocus(true);
        }

        private SlotUI CreateSlot()
        {
            return SlotUI.GetSlotUI<EquipmentItemSlot>(scroll.content);
        }

        public void SelectEquipmentItemUpdatable(DynamicEquipmentItemData item)
        {
            var targetItemSlot = _slotList.Where(slot => slot.HasData)
                .FirstOrDefault(slot => slot.GetData<DynamicEquipmentItemData>() == item);

            if (targetItemSlot)
            {
                SwapFocusSlot(targetItemSlot);
            }
        }

        public void UpdateUI()
        {
            foreach (var slotUI in _slotList.Where(slot => slot.gameObject.activeInHierarchy))
            {
                slotUI.RefreshUI();
            }
        }
    }
}