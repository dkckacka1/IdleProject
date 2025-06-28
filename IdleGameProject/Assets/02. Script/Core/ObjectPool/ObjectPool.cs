using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.ObjectPool
{
    public sealed class ObjectPool
    {
        private readonly Transform _poolParent;
        private readonly PoolableObject _originObject;
        private readonly Queue<PoolableObject> _poolQueue = new Queue<PoolableObject>();

        private const int DEFAULT_CREATE_COUNT = 10;

        private ObjectPool(Transform poolParent, PoolableObject originObject)
        {
            _poolParent = poolParent;
            _originObject = originObject;
        }

        public T Get<T>() where T : IPoolable
        {
            if (_poolQueue.Count <= 0)
                CreateObject();

            var poolObject = _poolQueue.Dequeue();
            poolObject.OnGet();
            poolObject.gameObject.SetActive(true);

            return poolObject.GetComponent<T>();
        }

        public void Release(PoolableObject poolable)
        {
            poolable.OnRelease();
            poolable.gameObject.SetActive(false);
            _poolQueue.Enqueue(poolable);
        }

        private void CreatePool(int createCount)
        {
            for (int i = 0; i < createCount; ++i)
            {
                CreateObject();
            }
        }

        private async UniTask CreatePoolAsync(int createCount)
        {
            var poolInstances = await Object.InstantiateAsync(_originObject, createCount, _poolParent, Vector3.zero,
                Quaternion.identity).ToUniTask();

            foreach (var instance in poolInstances)
            {
                instance.OnCreate();
                instance.gameObject.SetActive(false);
                _poolQueue.Enqueue(instance);
            }
        }

        private void CreateObject()
        {
            var poolInstance = Object.Instantiate(_originObject, _poolParent);
            poolInstance.OnCreate();
            poolInstance.gameObject.SetActive(false);
            _poolQueue.Enqueue(poolInstance);
        }

        public static ObjectPool GetInstance(Transform poolParent, PoolableObject originObject,
            int createCount = DEFAULT_CREATE_COUNT)
        {
            var pool = new ObjectPool(poolParent, originObject);
            pool.CreatePool(createCount);

            return pool;
        }

        public static async UniTask<ObjectPool> GetInstanceAsync(Transform poolParent, PoolableObject originObject, int createCount = DEFAULT_CREATE_COUNT)
        {
            var pool = new ObjectPool(poolParent, originObject);
            await pool.CreatePoolAsync(createCount);

            return pool;
        }
    }
}