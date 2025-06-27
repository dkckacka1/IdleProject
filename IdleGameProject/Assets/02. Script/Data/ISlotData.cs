using System;

namespace IdleProject.Data
{
    public interface ISlotData : IData
    {
        public string GetIconName { get; }
    }
}