using System.Collections.Generic;

namespace IdleProject.Battle.Character.EventGroup
{
    public class EventGroup<T>
    {
        public readonly List<IEventGroup<T>> EventGroupList = new();

        public void PublishAll(T publisher)
        {
            EventGroupList.ForEach(eventGroup => eventGroup.Publish(publisher));
        }

        public void UnPublishAll(T publisher)
        {
            EventGroupList.ForEach(eventGroup => eventGroup.UnPublish(publisher));
        }
    }
}