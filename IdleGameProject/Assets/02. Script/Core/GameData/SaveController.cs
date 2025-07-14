using System;
using System.Linq;
using IdleProject.Data;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using Newtonsoft.Json;
using UnityEngine;

namespace IdleProject.Core.GameData
{
    public class SaveController
    {
        private const string PLAYER_DATA_PLAYER_PREFS_NAME = "PLAYER_DATA_PLAYERPREFS";
        private const string DEFAULT_PLAYER_DATA_NAME = "DefaultPlayerData";
        
        private readonly PlayerData _playerData;

        public SaveController(PlayerData playerData)
        {
            _playerData = playerData;
        }

        public void Save(PlayerInfo playerInfo)
        {
            _playerData.PlayerInfo = playerInfo;
            
            SavePlayerData(_playerData);
        }
        
        public void Save(params IData[] dataParams)
        {
            foreach (var iData in dataParams)
            {
                if (iData is null)
                    continue;

                var dynamicData = iData.GetData<DynamicData>();
                if (dynamicData is not null)
                {
                    switch (dynamicData)
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

                    continue;
                }

                var staticData = iData.GetData<StaticData>();
                if (staticData is not null)
                {
                    switch (staticData)
                    {
                        case StaticStageData stageData:
                            SaveStageClear(stageData);
                            break;
                    }
                }
            }
            SavePlayerData(_playerData);
        }

        private void SaveCharacter(DynamicCharacterData characterData)
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

        private void SaveConsumableItem(DynamicConsumableItemData itemData)
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

        private void SaveEquipmentItem(DynamicEquipmentItemData itemData)
        {
            var playerItem = _playerData.playerEquipmentItemList.FirstOrDefault(item =>
                item.index == itemData.Index);

            if (playerItem is null)
                // 신규 아이템 추가
            {
                var newItem = new PlayerEquipmentItemData
                {
                    index = itemData.Index,
                    itemIndex = itemData.StaticData.itemName,
                };
                _playerData.playerEquipmentItemList.Add(newItem);
            }
            else
            {
                playerItem.equipmentCharacterName = itemData.EquipmentCharacterName;
            }
        }

        private void SaveStageClear(StaticStageData stageData)
        {
            _playerData.playerClearStageList.Add(stageData.Index);
        }

        public void SaveFormation(FormationInfo formation)
        {
            _playerData.frontLeftCharacterName = formation.frontLeftPositionInfo.characterName;
            _playerData.frontMiddleCharacterName = formation.frontMiddlePositionInfo.characterName;
            _playerData.frontRightCharacterName = formation.frontRightPositionInfo.characterName;
            _playerData.rearLeftCharacterName = formation.rearLeftPositionInfo.characterName;
            _playerData.rearRightCharacterName = formation.rearRightPositionInfo.characterName;
        }

        private static void SavePlayerData(PlayerData playerData)
        {
            var json = JsonConvert.SerializeObject(playerData);
            PlayerPrefs.SetString(PLAYER_DATA_PLAYER_PREFS_NAME, json);
        }

        public static PlayerData LoadPlayerData()
        {
            var json = PlayerPrefs.GetString(PLAYER_DATA_PLAYER_PREFS_NAME, string.Empty);

            try
            {
                var playerData =  JsonConvert.DeserializeObject<PlayerData>(json);
                return playerData;
            }
            catch (Exception e)
            {
                var playerData = Resources.Load<PlayerData>(DEFAULT_PLAYER_DATA_NAME);
                json = JsonConvert.SerializeObject(playerData);
            }
            
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }

        public static void ClearPlayerData()
        {
            PlayerPrefs.DeleteKey(PLAYER_DATA_PLAYER_PREFS_NAME);
        }
    }
}