using IdleProject.Core.Resource;
using IdleProject.Data;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using IdleProject.Util;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Core.UI.Slot
{
    public class EquipmentItemSlot : SlotParts
    {
        [SerializeField] private GameObject equipmentCharacterObject;
        [SerializeField] private Image equipmentCharacterIconImage;
        [SerializeField] private Image equipmentItemTypeImage;
        
        public void SetEquipmentCharacterIcon<T>(T data) where T : class ,ISlotData, IData
        {
            if (data is null)
            {
                SetEquipmentObject(false);
                return;
            }
            
            SetEquipmentObject(true);
            equipmentCharacterIconImage.sprite = ResourceManager.Instance.GetAsset<Sprite>(data.GetIconName);
        }

        private void SetEquipmentObject(bool isActive)
        {
            equipmentCharacterObject.SetActive(isActive);
        }

        public override void SetData<T>(T data)
        {
            equipmentCharacterIconImage.sprite = ResourceManager.Instance.GetAsset<Sprite>(data.GetIconName);
            if (data is StaticEquipmentItemData staticData)
            {
                equipmentItemTypeImage.sprite =
                    GetSpriteExtension.GetEquipmentTypeIconSprite(staticData.itemType);
            }
            else if (data is DynamicEquipmentItemData dynamicData)
            {
                equipmentItemTypeImage.sprite =
                    GetSpriteExtension.GetEquipmentTypeIconSprite(dynamicData.StaticData.itemType);
            }
        }
    }
}
