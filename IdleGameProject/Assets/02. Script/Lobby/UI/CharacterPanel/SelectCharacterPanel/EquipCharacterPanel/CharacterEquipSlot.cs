using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterEquipSlot : UIBase
    {
        [SerializeField] private GameObject defaultSlot;
        [SerializeField] private EquipmentItemSlot equipmentItemSlot;

        public SlotUI Slot => equipmentItemSlot.SlotUI;

        public void SetEquipmentItem(DynamicEquipmentItemData itemData)
        {
            defaultSlot.SetActive(itemData is null);
            equipmentItemSlot.gameObject.SetActive(itemData is not null);

            if (itemData is not null)
            {
                equipmentItemSlot.SlotUI.BindData(itemData);
                equipmentItemSlot.ShowParts(itemData);
            }
        }
    }
}

