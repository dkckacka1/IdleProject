using IdleProject.Core;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Data.StaticData
{
    [CreateAssetMenu(fileName = "EquipmentItemData", menuName = "Scriptable Objects/EquipmentItemData")]
    public class StaticEquipmentItemData : StaticItemData
    {
        public EquipmentItemType itemType;
        public CharacterStatType firstValueStatType;
        public float itemFirstValue;
        public CharacterStatType secondValueStatType;
        public float itemSecondValue;
    }
}