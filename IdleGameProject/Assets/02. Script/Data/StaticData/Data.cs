using UnityEngine;

namespace IdleProject.Data
{
    public abstract class Data : ScriptableObject
    {
        public abstract string Index { get; }
    }
}