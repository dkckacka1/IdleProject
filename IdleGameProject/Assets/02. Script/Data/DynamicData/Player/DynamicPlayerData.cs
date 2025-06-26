using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.DynamicData
{
    public class DynamicPlayerData
    {
        private readonly PlayerData _playerData;

        private readonly Dictionary<string, DynamicCharacterData> _playerCharacterDataDic;
        private readonly Dictionary<int, DynamicEquipmentItemData> _playerEquipmentItemDataDic;
        private readonly Dictionary<string, DynamicConsumableItemData> _playerConsumableItemDataDic;

        public DynamicPlayerData(PlayerData playerData)
        {
            _playerCharacterDataDic =
                playerData.playerCharacterList.ToDictionary
                (
                    data => data.characterName,
                    DynamicCharacterData.GetInstance
                );

            _playerEquipmentItemDataDic =
                playerData.playerEquipmentItemList.ToDictionary
                (
                    data => data.index,
                    DynamicEquipmentItemData.GetInstance
                );

            _playerConsumableItemDataDic =
                playerData.playerConsumableItemList.ToDictionary
                (
                    data => data.itemName,
                    DynamicConsumableItemData.GetInstance
                );

            _playerData = playerData;
        }

        public DynamicConsumableItemData GetItem(string itemName) => _playerConsumableItemDataDic[itemName];
        public DynamicEquipmentItemData GetItem(int index) => _playerEquipmentItemDataDic[index];
        public DynamicCharacterData GetCharacter(string characterName) => _playerCharacterDataDic[characterName];
        public List<DynamicCharacterData> GetCharacterList => _playerCharacterDataDic.Values.ToList();
        public List<DynamicEquipmentItemData> GetEquipmentItemList => _playerEquipmentItemDataDic.Values.ToList();

        public FormationInfo GetPlayerFormation()
        {
            var formation = new FormationInfo
            {
                frontMiddlePositionInfo = GetPosition(_playerData.frontMiddleCharacterName),
                frontLeftPositionInfo = GetPosition(_playerData.frontLeftCharacterName),
                frontRightPositionInfo = GetPosition(_playerData.frontRightCharacterName),
                rearLeftPositionInfo = GetPosition(_playerData.rearLeftCharacterName),
                rearRightPositionInfo = GetPosition(_playerData.rearRightCharacterName)
            };

            return formation;

            PositionInfo GetPosition(string characterName)
            {
                _playerCharacterDataDic.TryGetValue(characterName, out var character);
                
                var position = new PositionInfo
                {
                    characterName = character is not null ? characterName : string.Empty,
                    characterLevel = character?.Level ?? 0
                };

                return position;
            }
        }
    }
}