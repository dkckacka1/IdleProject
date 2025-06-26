using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.DynamicData
{
    public class DynamicEquipmentItemData : DynamicData<StaticEquipmentItemData>, ISlotData
    {
        public string equipmentCharacterName;
        public string GetIconName => StaticData.GetIconName;
        
        private DynamicEquipmentItemData(StaticEquipmentItemData equipmentItemData, string characterName) : base(equipmentItemData)
        {
            equipmentCharacterName = characterName;
        }

        public static DynamicEquipmentItemData GetInstance(PlayerEquipmentItemData playerEquipmentItemData)
        {
            var staticData = DataManager.Instance.GetData<StaticEquipmentItemData>(playerEquipmentItemData.itemName);
            return new DynamicEquipmentItemData(staticData, playerEquipmentItemData.equipmentCharacterName);
        }
    }
}