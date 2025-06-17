using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.Resource
{
    public abstract class AssetLoader<T> where T : Object
    {
        public abstract UniTask LoadAsset();
        
        public abstract T GetAsset(string address);
    }

}