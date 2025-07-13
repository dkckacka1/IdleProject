using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using IdleProject.Util;

namespace IdleProject.Data.DynamicData
{
    public class DynamicPlayerData
    {
        public PlayerInfo PlayerInfo;
        
        public readonly Dictionary<string, DynamicCharacterData> PlayerCharacterDataDic;
        public readonly Dictionary<string, DynamicEquipmentItemData> PlayerEquipmentItemDataDic;
        public readonly Dictionary<string, DynamicConsumableItemData> PlayerConsumableItemDataDic;
        public readonly HashSet<string> PlayerClearStageSet;
        
        public FormationInfo PlayerFormation;
        
        public DynamicPlayerData(PlayerData playerData)
        {
            PlayerInfo = playerData.PlayerInfo;
            
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
            
            PlayerCharacterDataDic =
                playerData.playerCharacterList.ToDictionary
                (
                    data => data.characterName,
                    DynamicCharacterData.GetInstance
                );

            PlayerClearStageSet = new HashSet<string>(playerData.playerClearStageList);
            
            PlayerFormation = GetPlayerFormation(playerData);
        }

        public void ClearStage(StaticStageData stageData)
        {
            var clearIndex = stageData.Index;

            if (PlayerClearStageSet.Add(clearIndex))
            {
                DataManager.Instance.SaveController.Save(stageData);
            }

            PlayerInfo.AddExp(stageData.playerExpAmount);
            DataManager.Instance.SaveController.Save(PlayerInfo);
        }

        public StaticStageData GetLastStage()
        {
            var lastClearStageIndex = PlayerClearStageSet.LastOrDefault();
            StaticStageData result = null;
            
            if (string.IsNullOrEmpty(lastClearStageIndex))
                // 클리어 스테이지가 없다면 첫번째 스테이지 반환
            {
                result = DataExtension.GetFirstStage();
            }
            else
                // 클리어 바로 다음 스테이지 
            {
                var lastClearStage = DataManager.Instance.GetData<StaticStageData>(lastClearStageIndex);
                DataExtension.TryGetNextStage(lastClearStage, out result);
            }

            return result;
        }

        public void AddConsumableItem(string index, int count = 1)
        {
            if (PlayerConsumableItemDataDic.TryGetValue(index, out var item))
            {
                item.itemCount += count;
            }
            else
            {
                PlayerConsumableItemDataDic.Add(index, DynamicConsumableItemData.GetInstance(index, count));
            }
            
            DataManager.Instance.SaveController.Save(PlayerConsumableItemDataDic[index]);
        }

        public void AddEquipmentItem(string itemIndex)
        {
            var key = PlayerDataExtension.GetNewGUID(itemIndex);
            var newItem = DynamicEquipmentItemData.GetInstance(itemIndex, key);
            PlayerEquipmentItemDataDic.Add(key, newItem);
            
            DataManager.Instance.SaveController.Save(newItem);
        }

        public void ChangePlayerFormation(List<(SpawnPositionType, string)> getPlayerFormation)
        {
            foreach (var value in getPlayerFormation)
            {
                switch (value.Item1)
                {
                    case SpawnPositionType.FrontMiddle:
                        PlayerFormation.frontMiddlePositionInfo.characterName = value.Item2;
                        break;
                    case SpawnPositionType.FrontRight:
                        PlayerFormation.frontRightPositionInfo.characterName = value.Item2;
                        break;
                    case SpawnPositionType.FrontLeft:
                        PlayerFormation.frontLeftPositionInfo.characterName = value.Item2;
                        break;
                    case SpawnPositionType.RearRight:
                        PlayerFormation.rearRightPositionInfo.characterName = value.Item2;
                        break;
                    case SpawnPositionType.RearLeft:
                        PlayerFormation.rearLeftPositionInfo.characterName = value.Item2;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            DataManager.Instance.SaveController.SaveFormation(PlayerFormation);
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