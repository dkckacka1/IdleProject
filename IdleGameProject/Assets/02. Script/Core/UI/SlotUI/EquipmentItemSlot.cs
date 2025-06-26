using IdleProject.Core.Resource;
using IdleProject.Data;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Core.UI.Slot
{
    public class EquipmentItemSlot : SlotComponent
    {
        [SerializeField] private GameObject equipmentCharacterObject;
        [SerializeField] private Image equipmentCharacterIconImage;

        public void SetEquipmentObject(bool isActive)
        {
            equipmentCharacterObject.SetActive(isActive);
        }
        
        public void SetIcon<T>(T data) where T : class ,ISlotData, IData
        {
            if (data is null)
            {
                SetEquipmentObject(false);
                return;
            }
            
            SetEquipmentObject(true);
            equipmentCharacterIconImage.sprite = ResourceManager.Instance.GetAsset<Sprite>(data.GetIconName);
        }
    }
}
