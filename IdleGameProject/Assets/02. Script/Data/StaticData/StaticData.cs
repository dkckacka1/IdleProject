using UnityEngine;

namespace IdleProject.Data.StaticData
{
    public abstract class StaticData : ScriptableObject, IData
    {
        public string Index => name;
        public T GetData<T>() where T : class, IData => this as T;
    }
}