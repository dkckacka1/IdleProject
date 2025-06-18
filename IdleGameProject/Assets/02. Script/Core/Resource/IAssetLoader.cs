using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.Resource
{
    public interface IAssetLoader<out T> where T : Object
    {
        public UniTask LoadAsset();
        
        public T GetAsset(string address);
    }
}