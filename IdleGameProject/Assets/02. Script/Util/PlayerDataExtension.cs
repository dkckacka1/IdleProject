using System;
using IdleProject.Core;
using IdleProject.Data.Player;

namespace IdleProject.Util
{
    public static class PlayerDataExtension
    {
        public static string GetNewGUID(string itemIndex)
        {
            return
                $"{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}_{itemIndex}_{Guid.NewGuid().ToString("N").Substring(0, 8)}";
        }

        
        public static string GetCharacterEquipmentItemIndex(this PlayerCharacterData playerCharacterData, EquipmentItemType itemType)
        {
            return itemType switch
            {
                EquipmentItemType.Weapon => playerCharacterData.equipmentWeaponIndex,
                EquipmentItemType.Helmet => playerCharacterData.equipmentHelmetIndex,
                EquipmentItemType.Armor => playerCharacterData.equipmentArmorIndex,
                EquipmentItemType.Glove => playerCharacterData.equipmentGloveIndex,
                EquipmentItemType.Boots => playerCharacterData.equipmentBootsIndex,
                EquipmentItemType.Accessory => playerCharacterData.equipmentAccessoryIndex,
                _ => throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null)
            };
        }
    }
}