using UnityEngine;

namespace IdleProject.Data
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "Scriptable Objects/EquipmentData")]
    public class EquipmentData : Data
    {
        public string equipmentName;
        
        public override string Index => equipmentName;
    }
}