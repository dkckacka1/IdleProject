using IdleProject.Core;
using IdleProject.Core.Resource;
using UnityEngine;

namespace IdleProject.Util
{
    public static class GetSpriteExtension
    {
        public static Sprite GetCharacterStatTypeIconSprite(CharacterStatType statType)
        {
            return ResourceManager.Instance.GetAsset<Sprite>($"Icon_CharacterStatType_{statType}");
        }
        
        public static Sprite GetEquipmentTypeIconSprite(EquipmentItemType itemType)
        {
            return ResourceManager.Instance.GetAsset<Sprite>($"Icon_EquipmentType_{itemType}");
        }
    }
}