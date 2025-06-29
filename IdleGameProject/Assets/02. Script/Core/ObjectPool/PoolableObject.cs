using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core.ObjectPool
{
    [RequireComponent(typeof(IPoolable))]
    public class PoolableObject : MonoBehaviour
    {
        [HideInInspector] public string address;

        public int defaultPoolCount = 10;

        private List<IPoolable> _poolableList;

        private void Awake()
        {
            _poolableList = new(GetComponents<IPoolable>());
        }

        public void OnGet()
        {
            foreach (var poolable in _poolableList)
            {
                poolable.OnGetAction();
            }
        }

        public void OnRelease()
        {
            foreach (var poolable in _poolableList)
            {
                poolable.OnReleaseAction();
            }
        }

        public void OnCreate()
        {
            foreach (var poolable in _poolableList)
            {
                poolable.OnCreateAction();
            }
        }
    }
}

