namespace IdleProject.Battle.Character.EventGroup
{
    public interface IEventGroup<in T>
    {
        public void Publish(T publisher);
        public void UnPublish(T publisher);
    }
}
