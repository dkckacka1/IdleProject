using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.Player
{
    [System.Serializable]
    public class PlayerEquipmentItemData
    {
        public int index;
        public string itemName;
        public string equipmentCharacterName;
        private ISlotData _slotDataImplementation;

        public StaticEquipmentItemData GetData => DataManager.Instance.GetData<StaticEquipmentItemData>(itemName);
    }
}