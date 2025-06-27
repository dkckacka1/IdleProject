using System.Linq;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using Palmmedia.ReportGenerator.Core.Parser.Analysis;

namespace IdleProject.Core.GameData
{
    public class SaveController
    {
        private readonly PlayerData _playerData;

        public SaveController()
        {
        }
        
        public SaveController(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void SaveAll(DynamicPlayerData playerData)
        {
            foreach (var character in playerData.PlayerCharacterDataDic.Values)
            {
                SaveCharacter(character);
            }

            foreach (var consumableItem in playerData.PlayerConsumableItemDataDic.Values)
            {
                SaveConsumableItem(consumableItem);
            }
        }
        
        public void Save(params DynamicData[] dataParams)
        {
            foreach (var data in dataParams)
            {
                if (data is null)
                    continue;
                
                switch (data)
                {
                    case DynamicCharacterData characterData:
                        SaveCharacter(characterData);
                        break;
                    case DynamicEquipmentItemData equipmentItemData:
                        SaveEquipmentItem(equipmentItemData);
                        break;
                    case DynamicConsumableItemData consumableItemData:
                        SaveConsumableItem(consumableItemData);
                        break;
                }
            }
        }

        public void SaveCharacter(DynamicCharacterData characterData)
        {
            var playerCharacter = _playerData.playerCharacterList.FirstOrDefault(character =>
                character.characterName == characterData.StaticData.Index);

            if (playerCharacter is null)
                // 캐릭터 추가
            {
                var newCharacter = new PlayerCharacterData
                {
                    characterName = characterData.StaticData.name,
                    level = characterData.Level,
                    exp = characterData.Exp,
                    
                    equipmentWeaponIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Weapon),
                    equipmentHelmetIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Helmet),
                    equipmentArmorIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Armor),
                    equipmentGloveIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Glove),
                    equipmentBootsIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Boots),
                    equipmentAccessoryIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Accessory),
                };
                _playerData.playerCharacterList.Add(newCharacter);
            }
            else
            {
                playerCharacter.level = characterData.Level;
                playerCharacter.exp = characterData.Exp;

                playerCharacter.equipmentWeaponIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Weapon);
                playerCharacter.equipmentHelmetIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Helmet);
                playerCharacter.equipmentArmorIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Armor);
                playerCharacter.equipmentGloveIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Glove);
                playerCharacter.equipmentBootsIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Boots);
                playerCharacter.equipmentAccessoryIndex = characterData.GetEquipmentItemIndex(EquipmentItemType.Accessory);
            }
        }

        public void SaveConsumableItem(DynamicConsumableItemData itemData)
        {
            var playerItem = _playerData.playerConsumableItemList.FirstOrDefault(item =>
                item.itemName == itemData.StaticData.Index);
            
            if (playerItem is null)
                // 신규 아이템 추가
            {
                var newItem = new PlayerConsumableItemData
                {
                    itemName = itemData.StaticData.Index,
                    itemCount = itemData.itemCount
                };
                _playerData.playerConsumableItemList.Add(newItem);
            }
            else
            {
                playerItem.itemCount = itemData.itemCount;
            }
        }

        public void SaveEquipmentItem(DynamicEquipmentItemData itemData)
        {
            var playerItem = _playerData.playerEquipmentItemList.FirstOrDefault(item =>
                item.index == itemData.Index);

            if (playerItem is null)
                // 신규 아이템 추가
            {
                var newItem = new PlayerEquipmentItemData
                {
                    index = itemData.Index,
                    itemName = itemData.StaticData.itemName,
                };
                _playerData.playerEquipmentItemList.Add(newItem);
            }
            else
            {
                playerItem.equipmentCharacterName = itemData.equipmentCharacterName;
            }
        }
    }
}