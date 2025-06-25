using UnityEngine;

namespace IdleProject.Data.StaticData
{
    public abstract class StaticData : ScriptableObject, IData
    {
        public abstract string Index { get; }
        public T GetData<T>() where T : class, IData => this as T;
    }
}