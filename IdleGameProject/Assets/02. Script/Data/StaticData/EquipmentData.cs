using UnityEngine;

namespace IdleProject.Data
{
    [CreateAssetMenu(fileName = "EquipmentData", menuName = "Scriptable Objects/EquipmentData")]
    public class EquipmentData : Data
    {
        public override string Index { get; }
    }
}