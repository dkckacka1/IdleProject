using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using IdleProject.Data.Player;

namespace IdleProject.Data.DynamicData
{
    public class DynamicPlayerData
    {
        public readonly Dictionary<string, DynamicCharacterData> PlayerCharacterDataDic;
        public readonly Dictionary<int, DynamicEquipmentItemData> PlayerEquipmentItemDataDic;
        public readonly Dictionary<string, DynamicConsumableItemData> PlayerConsumableItemDataDic;

        public FormationInfo PlayerFormation;
        
        public DynamicPlayerData(PlayerData playerData)
        {
            PlayerCharacterDataDic =
                playerData.playerCharacterList.ToDictionary
                (
                    data => data.characterName,
                    DynamicCharacterData.GetInstance
                );

            PlayerEquipmentItemDataDic =
                playerData.playerEquipmentItemList.ToDictionary
                (
                    data => data.index,
                    DynamicEquipmentItemData.GetInstance
                );

            PlayerConsumableItemDataDic =
                playerData.playerConsumableItemList.ToDictionary
                (
                    data => data.itemName,
                    DynamicConsumableItemData.GetInstance
                );
            
            PlayerFormation = GetPlayerFormation(playerData);
        }

        private FormationInfo GetPlayerFormation(PlayerData playerData)
        {
            var formation = new FormationInfo
            {
                frontMiddlePositionInfo = GetPosition(playerData.frontMiddleCharacterName),
                frontLeftPositionInfo = GetPosition(playerData.frontLeftCharacterName),
                frontRightPositionInfo = GetPosition(playerData.frontRightCharacterName),
                rearLeftPositionInfo = GetPosition(playerData.rearLeftCharacterName),
                rearRightPositionInfo = GetPosition(playerData.rearRightCharacterName)
            };

            return formation;

            PositionInfo GetPosition(string characterName)
            {
                PlayerCharacterDataDic.TryGetValue(characterName, out var character);
                
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