using UnityEngine;

namespace IdleProject.Data.StaticData
{
    public abstract class Data : ScriptableObject
    {
        public abstract string Index { get; }
    }
}