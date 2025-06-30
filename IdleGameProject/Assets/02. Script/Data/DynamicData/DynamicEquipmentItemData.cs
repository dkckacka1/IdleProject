using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.DynamicData
{
    public class DynamicEquipmentItemData : DynamicData<StaticEquipmentItemData>, ISlotData
    {
        public string Index;
        public string EquipmentCharacterName;
        public string GetIconName => StaticData.GetIconName;

        public DynamicCharacterData GetEquippedCharacter()
        {
            DynamicCharacterData result = null;
            
            if (string.IsNullOrEmpty(EquipmentCharacterName) is false)
            {
                result = DataManager.Instance.DataController.Player.PlayerCharacterDataDic[EquipmentCharacterName];
            }

            return result;
        }
        
        private DynamicEquipmentItemData(StaticEquipmentItemData equipmentItemData, string characterName, string index) : base(equipmentItemData)
        {
            EquipmentCharacterName = characterName;
            Index = index;
        }

        public static DynamicEquipmentItemData GetInstance(PlayerEquipmentItemData playerEquipmentItemData)
        {
            var staticData = DataManager.Instance.GetData<StaticEquipmentItemData>(playerEquipmentItemData.itemIndex);
            return new DynamicEquipmentItemData(staticData, playerEquipmentItemData.equipmentCharacterName, playerEquipmentItemData.index);
        }

        public static DynamicEquipmentItemData GetInstance(string itemIndex, string index)
        {
            var staticData = DataManager.Instance.GetData<StaticEquipmentItemData>(itemIndex);
            return new DynamicEquipmentItemData(staticData, string.Empty, index);
        }
    }
}