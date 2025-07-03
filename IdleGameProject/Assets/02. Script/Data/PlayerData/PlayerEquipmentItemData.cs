using IdleProject.Core.GameData;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.Player
{
    [System.Serializable]
    public class PlayerEquipmentItemData
    {
        public string index;
        public string itemIndex;
        public string equipmentCharacterName;

        public StaticEquipmentItemData GetData => DataManager.Instance.GetData<StaticEquipmentItemData>(itemIndex);
    }
}