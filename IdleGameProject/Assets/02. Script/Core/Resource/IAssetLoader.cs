using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.Resource
{
    public interface IAssetLoader<out T> where T : Object
    // ResourceManager에서 Object로 관리할 수 있도록 제네릭에 공변성 정의
    {
        public UniTask LoadAsset();
        
        public T GetAsset(string address);
    }
}