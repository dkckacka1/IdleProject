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

        public string frontMiddleCharacterName;
        public string frontRightCharacterName;
        public string frontLeftCharacterName;
        public string rearRightCharacterName;
        public string rearLeftCharacterName;
        
        
        [Button]
        private void CreateConsumableItem()
        {
            playerConsumableItemList.Clear();
            
            foreach (var itemData in DataManager.Instance.GetDataList<StaticConsumableItemData>())
            {
                playerConsumableItemList.Add(new PlayerConsumableItemData(itemData));
            }
        }
    }
}