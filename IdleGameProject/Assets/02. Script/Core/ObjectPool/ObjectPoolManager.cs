using Cysharp.Threading.Tasks;
using Engine.Core;
using Engine.Core.Addressable;
using Engine.Util;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core.ObjectPool
{
    public class ObjectPoolManager : SingletonMonoBehaviour<ObjectPoolManager>
    {
        Dictionary<string, Queue<PoolableObject>> poolableDic;

        private const int DefaultPoolCount = 10;
        private const int CreateCount = 5;
        private const int MaxPoolCap = 1000;

        private Transform defaultParent;

        public override void Initialized()
        {
            base.Initialized();
            poolableDic = new();

            defaultParent = new GameObject("defaultParent").transform;
            defaultParent.SetParent(transform);
        }

        public T Get<T>(string address, Transform parent = null) where T : IPoolable
        {
            if (!poolableDic.ContainsKey(address))
            {
                CreatePool<PoolableObject>(address);
            }

            var pool = poolableDic[address];
            if (pool.Count <= CreateCount)
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
            poolableDic[poolable.address].Enqueue(poolable);
        }

        public async UniTask CreatePool<T>(string address, Transform parent = null) where T : PoolableObject
        {
            if (poolableDic.ContainsKey(address)) return;

            poolableDic.Add(address, new());
            var pool = poolableDic[address];
            var poolableObj = await AddressableManager.Instance.LoadAssetAsync<PoolableObject>(address);

            for (int i = 0; i < poolableObj.defaultPoolCount; ++i)
            {
                await CreateObj<T>(address, pool, parent);
            }
        }

        private async UniTask CreateObj<T>(string address, Queue<PoolableObject> pool, Transform parent = null) where T : PoolableObject
        {
            if (parent is null)
            {
                parent = defaultParent;
            }

            var instObj = await AddressableManager.Instance.InstantiateObject<T>(address, parent);
            instObj.address = address;
            instObj.OnCreate();
            instObj.gameObject.SetActive(false);
            pool.Enqueue(instObj);
        }
    }
}
