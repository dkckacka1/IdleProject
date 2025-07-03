using System.Collections.Generic;
using System.Linq;
using Engine.Util.Extension;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using IdleProject.Util;
using UnityEngine;

namespace IdleProject.Data.DynamicData
{
    public class DynamicCharacterData : DynamicData<StaticCharacterData>, ISlotData
    {
        public int Level;
        public int Exp;

        private readonly Dictionary<EquipmentItemType, string> _equippedItemDic = new();
        
        public string GetIconName => StaticData.GetIconName;
        public int GetLevelUpExpValue => Level * 100;

        public int GetCombatPower()
        {
            var stat = GetStat();
            var power = stat.healthPoint * 0.4f + stat.attackDamage * 2.0f + stat.defensePoint * 0.8f + stat.criticalPercent * 1.5f + stat.criticalResistance * 0.3f;
            return Mathf.RoundToInt(power * 3);
        }
        
        private DynamicCharacterData(int characterLevel, int exp, StaticCharacterData staticCharacterData) : base(staticCharacterData)
        {
            EnumExtension.Foreach<EquipmentItemType>(type =>
            {
                _equippedItemDic.Add(type, string.Empty);
            });
            
            Level = characterLevel;
            Exp = exp;
        }

        public StatValue GetStat()
        {
            bool notDefaultLevel = Level > 1; 
            
            var statValue = new StatValue
            {
                healthPoint = StaticData.stat.healthPoint + GetEquipmentStatValue(CharacterStatType.HealthPoint) + (notDefaultLevel ? StaticData.levelValue.healthPointValue * (Level - 1) : 0),
                manaPoint = StaticData.stat.manaPoint + GetEquipmentStatValue(CharacterStatType.ManaPoint),
                attackDamage = StaticData.stat.attackDamage + GetEquipmentStatValue(CharacterStatType.AttackDamage) + (notDefaultLevel ? StaticData.levelValue.attackDamageValue * (Level - 1) : 0),
                movementSpeed = StaticData.stat.movementSpeed + GetEquipmentStatValue(CharacterStatType.MovementSpeed),
                attackRange = StaticData.stat.attackRange + GetEquipmentStatValue(CharacterStatType.AttackRange),
                attackCoolTime = StaticData.stat.attackCoolTime + GetEquipmentStatValue(CharacterStatType.AttackCoolTime),
                defensePoint = StaticData.stat.defensePoint + GetEquipmentStatValue(CharacterStatType.DefensePoint),
                criticalPercent = StaticData.stat.criticalPercent + GetEquipmentStatValue(CharacterStatType.CriticalPercent),
                criticalResistance = StaticData.stat.criticalResistance + GetEquipmentStatValue(CharacterStatType.CriticalResistance)
            };
            
            return statValue;
        }

        public float GetEquipmentStatValue(CharacterStatType statType)
        {
            float result = 0f;

            var equippedItemList = _equippedItemDic.Values.Where(index => string.IsNullOrEmpty(index) is false).Select(index =>
                DataManager.Instance.DataController.Player.PlayerEquipmentItemDataDic[index]);
            

            foreach (var equipmentItem in equippedItemList)
            {
                if (equipmentItem.StaticData.firstValueStatType == statType)
                {
                    result += equipmentItem.StaticData.itemFirstValue;
                }
                
                if (equipmentItem.StaticData.secondValueStatType == statType)
                {
                    result += equipmentItem.StaticData.itemSecondValue;
                }
            }
            
            return result;
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

        public DynamicEquipmentItemData GetEquipmentItem(EquipmentItemType itemType)
        {
            DynamicEquipmentItemData result = null;
            var index = _equippedItemDic[itemType];

            if (string.IsNullOrEmpty(index) is false)
            {
                result = DataManager.Instance.DataController.Player.PlayerEquipmentItemDataDic[index];
            }

            return result;
        }

        public string GetEquipmentItemIndex(EquipmentItemType itemType)
        {
            return _equippedItemDic[itemType];
        }
        
        // 장비 장착
        public void SetEquipmentItem(EquipmentItemType itemType, DynamicEquipmentItemData equipmentItemData)
        {
            // 기존 장비 해제
            ReleaseEquipmentItem(itemType);
            
            // 신규 장비 장착
            equipmentItemData.EquipmentCharacterName = StaticData.Index;
            _equippedItemDic[itemType] = equipmentItemData.Index;
        }

        // 장비 해제
        public void ReleaseEquipmentItem(EquipmentItemType itemType)
        {
            var itemData = GetEquipmentItem(itemType);
            if (itemData is not null)
            {
                itemData.EquipmentCharacterName = string.Empty;
            }
            
            _equippedItemDic[itemType] = string.Empty;
        }
        
        private void EquipAllItem(PlayerCharacterData characterData)
        {
            EnumExtension.Foreach<EquipmentItemType>(type =>
            {
                _equippedItemDic[type] = characterData.GetCharacterEquipmentItemIndex(type);
            });
        }
        
        public static int GetLevelExpValue(int level) => level * 100;



        #region Factory

        public static DynamicCharacterData GetInstance(PlayerCharacterData characterData)
        {
            var staticCharacterData = DataManager.Instance.GetData<StaticCharacterData>(characterData.characterName);
            var dataInstance = new DynamicCharacterData(characterData.level, characterData.exp, staticCharacterData);
            dataInstance.EquipAllItem(characterData);

            return dataInstance;
        }

        public static DynamicCharacterData GetInstance(PositionInfo info)
        {
            var staticCharacterData = DataManager.Instance.GetData<StaticCharacterData>(info.characterName);
            
            return new DynamicCharacterData(info.characterLevel, 0, staticCharacterData);
        }

        #endregion

    }
}