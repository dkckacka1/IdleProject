using System.Collections.Generic;
using System.Linq;
using IdleProject.Core.GameData;
using IdleProject.Data.StaticData;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Create/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public int playerLevel = 1;
        public int playerExp = 0;

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
                index = playerEquipmentItemList.Count + 1,
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
                    index = playerEquipmentItemList.Count + 1,
                    itemIndex = itemData.Index
                };
                playerEquipmentItemList.Add(newItem);
            }
        }
    }
}