using IdleProject.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data.StaticData
{
    [CreateAssetMenu(fileName = "EquipmentItemData", menuName = "Scriptable Objects/EquipmentItemData")]
    public class StaticEquipmentItemData : StaticItemData
    {
        public EquipmentItemType itemType;
        public float itemValue01;
        public float itemValue02;
    }
}