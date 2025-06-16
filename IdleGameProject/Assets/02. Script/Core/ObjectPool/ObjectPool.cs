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

        private const uint DEFAULT_CREATE_COUNT = 10;

        public ObjectPool(Transform poolParent, PoolableObject originObject, uint createCount = DEFAULT_CREATE_COUNT)
        {
            _poolParent = poolParent;
            _originObject = originObject;
            CreatePool(createCount);
        }

        public T Get<T>() where T : IPoolable
        {
            if(_poolQueue.Count <= 0)
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
        
        private void CreatePool(uint createCount)
        {
            for (int i = 0; i < createCount; ++i)
            {
                CreateObject();
            }
        }

        private void CreateObject()
        {
            var poolInstance = Object.Instantiate(_originObject, _poolParent);
            poolInstance.OnCreate();
            poolInstance.gameObject.SetActive(false);
            _poolQueue.Enqueue(poolInstance);
        }
    }
}