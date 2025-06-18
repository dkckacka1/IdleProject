using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.Resource
{
    public abstract class AssetLoader<T> : IAssetLoader<T> where T: Object
    {
        protected readonly Dictionary<string, T> AssetCacheDic = new();
        public abstract UniTask LoadAsset();
        public abstract T GetAsset(string address);
    }
}