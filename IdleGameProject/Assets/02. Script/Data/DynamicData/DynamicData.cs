namespace IdleProject.Data.DynamicData
{
    public abstract class DynamicData : IData
    {
        public T GetData<T>() where T : class, IData => this as T;
    }
}