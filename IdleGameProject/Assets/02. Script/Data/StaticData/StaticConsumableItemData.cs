using IdleProject.Core;
using UnityEngine;

namespace IdleProject.Data.StaticData
{

    [CreateAssetMenu(fileName = "ConsumableItemData", menuName = "Scriptable Objects/ConsumableItemData")]
    public class StaticConsumableItemData : StaticItemData
    {
        public ConsumableType consumableType;
        public int value;
    }
}