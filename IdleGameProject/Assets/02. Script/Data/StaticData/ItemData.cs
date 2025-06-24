using IdleProject.Data;

namespace IdleProject.Data.StaticData
{
    public abstract class ItemData : Data, ISlotData
    {
        public string itemName;
        public override string Index => itemName;
        public virtual string GetIconName => $"Icon_{itemName}";
    }

}
