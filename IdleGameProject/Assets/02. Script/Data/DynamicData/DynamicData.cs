namespace IdleProject.Data.DynamicData
{
    public abstract class DynamicData : IData
    {
        public TData GetData<TData>() where TData : class, IData
        {
            return this as TData;
        }
    }
    
    public abstract class DynamicData<T> : DynamicData where T : StaticData.StaticData
    {
        public readonly T StaticData;

        protected DynamicData(T staticData)
        {
            StaticData = staticData;
        }
    }
}