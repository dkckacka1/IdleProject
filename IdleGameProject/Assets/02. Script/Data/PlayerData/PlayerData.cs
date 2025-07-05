using System.Collections.Generic;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Data.StaticData;
using IdleProject.Util;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Data.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Create/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public PlayerInfo PlayerInfo;
        
        public List<PlayerCharacterData> playerCharacterList;
        public List<PlayerConsumableItemData> playerConsumableItemList;
        public List<PlayerEquipmentItemData> playerEquipmentItemList;
        public List<string> playerClearStageList;

        public string frontMiddleCharacterName;
        public string frontRightCharacterName;
        public string frontLeftCharacterName;
        public string rearRightCharacterName;
        public string rearLeftCharacterName;


        private bool IsGamePlay => Application.isPlaying;
        [BoxGroup("Test"), Button, ShowIf("@this.IsGamePlay")]
        private void CreateConsumableItem()
        {
            playerConsumableItemList.Clear();

            foreach (var itemData in DataManager.Instance.GetDataList<StaticConsumableItemData>())
            {
                var item = new PlayerConsumableItemData();
                item.itemName = itemData.Index;
                playerConsumableItemList.Add(item);
            }
        }

        [BoxGroup("Test/EquipmentItem"), SerializeField, ShowIf("@this.IsGamePlay")]
        private StaticEquipmentItemData equipmentItemData;

        [BoxGroup("Test/EquipmentItem"), Button, ShowIf("@this.IsGamePlay")]
        private void CreateEquipmentItem()
        {
            if (equipmentItemData is null) return;

            var newItem = new PlayerEquipmentItemData
            {
                index = PlayerDataExtension.GetNewGUID(equipmentItemData.Index),
                itemIndex = equipmentItemData.Index
            };
            playerEquipmentItemList.Add(newItem);
        }

        [BoxGroup("Test/EquipmentItem"), Button, ShowIf("@this.IsGamePlay")]
        private void CreateAllEquipmentItem()
        {
            playerEquipmentItemList.Clear();

            foreach (var itemData in DataManager.Instance.GetDataList<StaticEquipmentItemData>())
            {
                var newItem = new PlayerEquipmentItemData
                {
                    index = PlayerDataExtension.GetNewGUID(equipmentItemData.Index),
                    itemIndex = itemData.Index
                };
                playerEquipmentItemList.Add(newItem);
            }
        }
    }
}