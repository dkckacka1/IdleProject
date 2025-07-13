using IdleProject.Data.StaticData;

namespace IdleProject.Data.SerializableData
{
    [System.Serializable]
    public class RewardData
    {
        public StaticItemData itemData;
        public int count = 1;
    }
}