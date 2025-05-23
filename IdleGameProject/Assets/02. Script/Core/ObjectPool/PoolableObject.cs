using System.Collections.Generic;
using UnityEngine;

namespace IdleProject.Core.ObjectPool
{
    [RequireComponent(typeof(IPoolable))]
    public class PoolableObject : MonoBehaviour
    {
        [HideInInspector] public string address;

        private List<IPoolable> poolableList;

        private void Awake()
        {
            poolableList = new(GetComponents<IPoolable>());
        }

        public void OnGet()
        {
            foreach (var poolable in poolableList)
            {
                poolable.OnGetAction();
            }
        }

        public void OnRelease()
        {
            foreach (var poolable in poolableList)
            {
                poolable.OnReleaseAction();
            }
        }

        public void OnCreate()
        {
            foreach (var poolable in poolableList)
            {
                poolable.OnCreateAction();
            }
        }
    }
}

