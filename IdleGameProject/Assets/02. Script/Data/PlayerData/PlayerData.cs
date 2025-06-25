using System.Collections.Generic;
using System.Linq;
using IdleProject.Core.GameData;
using IdleProject.Data.StaticData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Data.Player
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "Create/PlayerData")]
    public class PlayerData : ScriptableObject
    {
        public int playerLevel = 1;
        public int playerExp = 0;
        
        public List<PlayerCharacterData> userCharacterList;
        public List<PlayerConsumableItemData> userConsumableItemList;

        public string frontMiddleCharacterName;
        public string frontRightCharacterName;
        public string frontLeftCharacterName;
        public string rearRightCharacterName;
        public string rearLeftCharacterName;

        public PlayerCharacterData GetCharacter(string characterName) =>
            userCharacterList.FirstOrDefault(data => data.characterName == characterName);
        
        public PlayerConsumableItemData GetItem(string itemName) => userConsumableItemList.FirstOrDefault(data => data.itemName == itemName);
        
        [Button]
        private void CreateConsumableItem()
        {
            userConsumableItemList.Clear();
            
            foreach (var itemData in DataManager.Instance.GetDataList<StaticConsumableItemData>())
            {
                userConsumableItemList.Add(new PlayerConsumableItemData(itemData));
            }
        }
    }
}