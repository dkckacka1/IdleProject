using Cysharp.Threading.Tasks;
using Engine.Util;
using System.Collections.Generic;
using IdleProject.Core.Resource;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IdleProject.Core.ObjectPool
{
    public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager>
    {
        private Dictionary<string, ObjectPool> _poolableDic;

        private Transform _defaultParent;

        protected override void Initialized()
        {
            base.Initialized();
            _poolableDic = new();

            _defaultParent = new GameObject("defaultParent").transform;
            _defaultParent.SetParent(transform);

            SceneManager.sceneLoaded += CleatObjectPool;
        }
        public T Get<T>(string address) where T : IPoolable
        {
            var pool = _poolableDic[address];

            return pool.Get<T>();
        }

        public void Release(PoolableObject poolable)
        {
            _poolableDic[poolable.address].Release(poolable);
        }
        
        public void CreatePool(string address, Transform parent = null)
        {
            if (_poolableDic.ContainsKey(address)) return;

            parent ??= _defaultParent;
            
            var poolableObj = ResourceManager.Instance.GetPrefab(ResourceManager.PoolableObject, address).GetComponent<PoolableObject>();
            poolableObj.address = address;
            _poolableDic.Add(address, ObjectPool.GetInstance(parent, poolableObj, poolableObj.defaultPoolCount));
        }
        
        public async UniTask CreatePoolAsync(string address, Transform parent = null)
        {
            if (_poolableDic.ContainsKey(address)) return;

            parent ??= _defaultParent;
            
            var poolableObj = ResourceManager.Instance.GetPrefab(ResourceManager.PoolableObject, address).GetComponent<PoolableObject>();
            poolableObj.address = address;

            var objectPool = await ObjectPool.GetInstanceAsync(parent, poolableObj, poolableObj.defaultPoolCount);
            _poolableDic.Add(address, objectPool);
        }


        private void CleatObjectPool(Scene scene, LoadSceneMode sceneMode)
        {
            _poolableDic.Clear();
        }
    }
}
