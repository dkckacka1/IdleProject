using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using Sirenix.OdinInspector.Editor;

namespace IdleProject.Data.DynamicData
{
    public class DynamicPlayerData
    {
        public readonly PlayerData PlayerData;
        public readonly List<DynamicCharacterData> PlayerCharacterDataList = new();

        public DynamicPlayerData(PlayerData playerData)
        {
            foreach (var playerCharacterData in playerData.userCharacterList)
            {
                PlayerCharacterDataList.Add(DynamicCharacterData.GetInstance(playerCharacterData));
            }

            PlayerData = playerData;
        }

        public FormationInfo GetPlayerFormation()
        {
            var formation = new FormationInfo
            {
                frontMiddlePositionInfo = GetPosition(PlayerData.frontMiddleCharacterName),
                frontLeftPositionInfo = GetPosition(PlayerData.frontLeftCharacterName),
                frontRightPositionInfo = GetPosition(PlayerData.frontRightCharacterName),
                rearLeftPositionInfo = GetPosition(PlayerData.rearLeftCharacterName),
                rearRightPositionInfo = GetPosition(PlayerData.rearRightCharacterName)
            };

            return formation;

            PositionInfo GetPosition(string characterName)
            {
                var character = PlayerData.GetCharacter(characterName);
                
                var position = new PositionInfo
                {
                    characterName = character is not null ? characterName : string.Empty,
                    characterLevel = character?.level ?? 0
                };

                return position;
            }
        }
    }
}