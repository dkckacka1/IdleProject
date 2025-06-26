namespace IdleProject.Data.DynamicData
{
    public abstract class DynamicData<T> : IData where T : StaticData.StaticData
    {
        public readonly T StaticData;

        protected DynamicData(T staticData)
        {
            StaticData = staticData;
        }

        public TData GetData<TData>() where TData : class, IData
        {
            return this as TData;
        }
    }
}