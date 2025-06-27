using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.DynamicData
{
    public class DynamicEquipmentItemData : DynamicData<StaticEquipmentItemData>, ISlotData
    {
        public int Index;
        public string equipmentCharacterName;
        public string GetIconName => StaticData.GetIconName;
        
        private DynamicEquipmentItemData(StaticEquipmentItemData equipmentItemData, string characterName, int index) : base(equipmentItemData)
        {
            equipmentCharacterName = characterName;
            Index = index;
        }

        public static DynamicEquipmentItemData GetInstance(PlayerEquipmentItemData playerEquipmentItemData)
        {
            var staticData = DataManager.Instance.GetData<StaticEquipmentItemData>(playerEquipmentItemData.itemName);
            return new DynamicEquipmentItemData(staticData, playerEquipmentItemData.equipmentCharacterName, playerEquipmentItemData.index);
        }
    }
}