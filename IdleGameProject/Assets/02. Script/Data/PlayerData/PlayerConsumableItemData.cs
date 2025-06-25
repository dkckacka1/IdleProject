using IdleProject.Data.StaticData;
using UnityEngine;

namespace IdleProject.Data.Player
{
    [System.Serializable]
    public class PlayerConsumableItemData
    {
        public string itemName;
        public int itemCount;

        public PlayerConsumableItemData(StaticConsumableItemData item, int count = 0)
        {
            itemName = item.itemName;
            itemCount = count;
        }
    } 
}