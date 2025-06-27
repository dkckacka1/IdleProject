using IdleProject.Core.GameData;
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
        
        public void SetEquipmentCharacterIcon(StaticCharacterData data)
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

        public override void ShowParts<T>(T data)
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

                var isEquippedCharacter = string.IsNullOrEmpty(dynamicData.equipmentCharacterName) is false;
                var characterData = isEquippedCharacter
                    ? DataManager.Instance.GetData<StaticCharacterData>(dynamicData.equipmentCharacterName)
                    : null;
                
                SetEquipmentCharacterIcon(characterData);
            }
        }
    }
}
