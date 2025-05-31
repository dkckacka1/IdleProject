using Cysharp.Threading.Tasks;
using Engine.Core.Addressable;
using Engine.Util;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace IdleProject.Core.ObjectPool
{
    public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager>
    {
        private Dictionary<string, Queue<PoolableObject>> _poolableDic;

        private const int DEFAULT_POOL_COUNT = 10;
        private const int CREATE_COUNT = 5;
        private const int MAX_POOL_CAP = 1000;

        private Transform _defaultParent;

        protected override void Initialized()
        {
            base.Initialized();
            _poolableDic = new();

            _defaultParent = new GameObject("defaultParent").transform;
            _defaultParent.SetParent(transform);
        }

        public T Get<T>(string address, Transform parent = null) where T : IPoolable
        {
            if (!_poolableDic.ContainsKey(address))
            {
                CreatePool<PoolableObject>(address);
            }

            var pool = _poolableDic[address];
            if (pool.Count <= CREATE_COUNT)
            {
                CreateObj<PoolableObject>(address, pool, parent).Forget();
            }

            var getObj = pool.Dequeue();
            getObj.OnGet();
            getObj.gameObject.SetActive(true);

            return getObj.GetComponent<T>();
        }

        public void Release(PoolableObject poolable)
        {
            poolable.OnRelease();
            poolable.gameObject.SetActive(false);
            _poolableDic[poolable.address].Enqueue(poolable);
        }

        public async UniTask CreatePool<T>(string address, Transform parent = null) where T : PoolableObject
        {
            if (_poolableDic.ContainsKey(address)) return;

            _poolableDic.Add(address, new());
            var pool = _poolableDic[address];
            var poolableObj = await AddressableManager.Instance.LoadAssetAsync<PoolableObject>(address);

            for (int i = 0; i < poolableObj.defaultPoolCount; ++i)
            {
                await CreateObj<T>(address, pool, parent);
            }
        }

        private async UniTask CreateObj<T>(string address, Queue<PoolableObject> pool, Transform parent = null) where T : PoolableObject
        {
            parent ??= _defaultParent;
            var instObj = await AddressableManager.Instance.InstantiateObject<T>(address, parent, container: ProjectContext.Instance.Container);
            
            instObj.address = address;
            instObj.OnCreate();
            instObj.gameObject.SetActive(false);
            pool.Enqueue(instObj);
        }
    }
}
