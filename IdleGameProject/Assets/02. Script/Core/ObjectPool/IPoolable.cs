namespace IdleProject.Core.ObjectPool
{
    public interface IPoolable
    {
        public void OnGetAction();
        public void OnCreateAction();
        public void OnReleaseAction();
    }
}