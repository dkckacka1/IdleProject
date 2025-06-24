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
        
        public List<PlayerHeroData> userHeroList;
        public List<PlayerConsumableItemData> userConsumableItemList;

        public FormationInfo userFormation;

        public PlayerConsumableItemData GetItem(string itemName)
        {
            return userConsumableItemList.FirstOrDefault(data => data.itemName == itemName);
        }
        
        [Button]
        private void CreateConsumableItem()
        {
            userConsumableItemList.Clear();
            
            foreach (var itemData in DataManager.Instance.GetDataList<ConsumableItemData>())
            {
                userConsumableItemList.Add(new PlayerConsumableItemData(itemData));
            }
        }
    }
}