using System;
using System.Collections.Generic;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.DynamicData
{
    public class DynamicCharacterData : DynamicData<StaticCharacterData>, ISlotData
    {
        public StatValue Stat;
        public int Level;
        public int Exp;

        public DynamicEquipmentItemData Helmet;
        public DynamicEquipmentItemData Weapon;
        public DynamicEquipmentItemData Armor;
        public DynamicEquipmentItemData Glove;
        public DynamicEquipmentItemData Boots;
        public DynamicEquipmentItemData Accessory;
        
        public string GetIconName => StaticData.GetIconName;
        public int GetLevelUpExpValue => Level * 100;
        private DynamicCharacterData(int characterLevel, int exp, StaticCharacterData staticCharacterData) : base(staticCharacterData)
        {
            Level = characterLevel;
            Exp = exp;
            SetStat(characterLevel, staticCharacterData);
        }

        private void EquipItem(out DynamicEquipmentItemData targetEquipmentItem, int equipmentItemIndex)
        {
            targetEquipmentItem = DataManager.Instance.DataController.Player.PlayerEquipmentItemDataDic[equipmentItemIndex];
        }
        
        private void EquipAllItem(PlayerCharacterData characterData)
        {
            EquipItem(out Weapon, characterData.equipmentWeaponIndex);
            EquipItem(out Helmet, characterData.equipmentHelmetIndex);
            EquipItem(out Armor, characterData.equipmentArmorIndex);
            EquipItem(out Glove, characterData.equipmentGloveIndex);
            EquipItem(out Accessory, characterData.equipmentAccessoryIndex);
            EquipItem(out Boots, characterData.equipmentBootsIndex);
        }

        public DynamicEquipmentItemData GetEquipItem(EquipmentItemType equipmentItemType)
        {
            return equipmentItemType switch
            {
                EquipmentItemType.Weapon => Weapon,
                EquipmentItemType.Helmet => Helmet,
                EquipmentItemType.Armor => Armor,
                EquipmentItemType.Glove => Glove,
                EquipmentItemType.Boots => Boots,
                EquipmentItemType.Accessory => Accessory,
                _ => throw new ArgumentOutOfRangeException(nameof(equipmentItemType), equipmentItemType, null)
            };
        }

        public void UpdateCharacter(int characterLevel)
        {
            SetStat(characterLevel, StaticData);
        }
        
        public void AddExp(int expAmount)
        {
            Exp += expAmount;
            while (Exp > GetLevelUpExpValue)
            {
                Exp -= GetLevelUpExpValue; 
                ++Level;
            }
        }

        private void SetStat(int characterLevel, StaticCharacterData staticCharacterData)
        {
            var levelData = staticCharacterData.levelValue;
            Stat = staticCharacterData.stat;
            Stat.attackDamage += characterLevel > 1 ? levelData.attackDamageValue * (characterLevel - 1) : 0;
            Stat.healthPoint += characterLevel > 1 ? levelData.healthPointValue * (characterLevel - 1) : 0;
        }

        private List<DynamicEquipmentItemData> GetEquippedItemList()
        {
            return new List<DynamicEquipmentItemData>(new []{Helmet, Weapon, Armor, Glove, Boots, Accessory});
        }

        public static int GetLevelExpValue(int level) => level * 100;
        
        public static DynamicCharacterData GetInstance(PlayerCharacterData characterData)
        {
            var staticCharacterData = DataManager.Instance.GetData<StaticCharacterData>(characterData.characterName);

            return new DynamicCharacterData(characterData.level, characterData.exp, staticCharacterData);
        }

        public static DynamicCharacterData GetInstance(PositionInfo info)
        {
            var staticCharacterData = DataManager.Instance.GetData<StaticCharacterData>(info.characterName);
            
            return new DynamicCharacterData(info.characterLevel, 0, staticCharacterData);
        }
    }
}