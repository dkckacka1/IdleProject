using UnityEngine;

namespace IdleProject.Data.StaticData
{
    public enum ConsumableType
    {
        CharacterExp,
    }
    
    [CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Scriptable Objects/ConsumableItemData")]
    public class ConsumableItemData : ItemData
    {
        public ConsumableType consumableType;
        public int value;
    }
}