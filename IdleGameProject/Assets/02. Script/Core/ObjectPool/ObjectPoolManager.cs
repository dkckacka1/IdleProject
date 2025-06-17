using Cysharp.Threading.Tasks;
using Engine.Core.Addressable;
using Engine.Util;
using System.Collections.Generic;
using IdleProject.Battle.UI;
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

        public async UniTask CreatePool<T>(string address, Transform parent = null) where T : PoolableObject
        {
            if (_poolableDic.ContainsKey(address)) return;

            if (parent is null)
                parent = _defaultParent;
            
            var poolableObj = await AddressableManager.Instance.Controller.LoadAssetAsync<PoolableObject>(address);
            poolableObj.address = address;
            _poolableDic.Add(address, new ObjectPool(parent , poolableObj, poolableObj.defaultPoolCount));
        }

        private void CleatObjectPool(Scene arg0, LoadSceneMode arg1)
        {
            _poolableDic.Clear();
        }
    }
}
