namespace IdleProject.Data.StaticData
{
    public abstract class StaticItemData : StaticData, ISlotData
    {
        public string itemName;
        public virtual string GetIconName => $"Icon_{Index}";
    }

}
