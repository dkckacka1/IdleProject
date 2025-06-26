using IdleProject.Core.GameData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Data.DynamicData
{
    public class DynamicConsumableItemData : DynamicData<StaticConsumableItemData>, ISlotData
    {
        public int itemCount = 0;

        public string GetIconName => StaticData.GetIconName;

        private DynamicConsumableItemData(StaticConsumableItemData staticData, int count) : base(staticData)
        {
            itemCount = count;
        }

        public static DynamicConsumableItemData GetInstance(PlayerConsumableItemData data)
        {
            var staticData = DataManager.Instance.GetData<StaticConsumableItemData>(data.itemName);

            return new DynamicConsumableItemData(staticData, data.itemCount);
        }
    }
}