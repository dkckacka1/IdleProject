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

        #region Factory
        public static DynamicConsumableItemData GetInstance(PlayerConsumableItemData data)
        {
            var staticData = DataManager.Instance.GetData<StaticConsumableItemData>(data.itemName);

            return new DynamicConsumableItemData(staticData, data.itemCount);
        }

        public static DynamicConsumableItemData GetInstance(string itemIndex, int count = 1)
        {
            var staticData = DataManager.Instance.GetData<StaticConsumableItemData>(itemIndex);

            return new DynamicConsumableItemData(staticData, count);
        }

        #endregion

    }
}