namespace IdleProject.Data
{
    public interface IData
    {
        public T GetData<T>() where T : class, IData;
    }
}