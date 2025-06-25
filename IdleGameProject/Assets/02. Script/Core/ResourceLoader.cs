using System;
using Cysharp.Threading.Tasks;
using IdleProject.Battle;
using IdleProject.Core.ObjectPool;
using UnityEngine;

namespace IdleProject.Core
{

    public static class ResourceLoader
    {
        private const string PREFAB_PATH = "Prefab";
        private const string POOLABLE_PATH = "Poolable";
        private const string MODEL_PATH = "Model";

        private const string PREFAB_EXTENSION = "prefab";

        private const char PATH_SEGMENT = '/';

        public static async UniTask<GameObject> InstantiateCharacterModel(string characterName, Transform parent)
        {
            var prefabName = $"{MODEL_PATH}_{characterName}";
            var address = $"{JoinSegment(PATH_SEGMENT, PREFAB_PATH, nameof(ResourceType.Character), MODEL_PATH, prefabName)}.{PREFAB_EXTENSION}";
            var model = await AddressableManager.Instance.Controller.InstantiateObject<GameObject>(address, parent);

            model.transform.position = parent.transform.position;
            return model;
        }
        
        public static async UniTask<T> InstantiateUI<T>(SceneType sceneType, string name) where T : Component
        {
            var address =
                $"{JoinSegment(PATH_SEGMENT, PREFAB_PATH, nameof(ResourceType.UI), sceneType.ToString(), name)}.{PREFAB_EXTENSION}";
            var uiObj = await AddressableManager.Instance.Controller.InstantiateObject<GameObject>(address);
            return uiObj.GetComponent<T>();
        }
        
        public static T GetPoolableObject<T>(PoolableType poolableType, string name) where T : IPoolable
        {
            var address = $"{JoinSegment(PATH_SEGMENT, POOLABLE_PATH, poolableType.ToString(), name)}.{PREFAB_EXTENSION}";
            return ObjectPoolManager.Instance.Get<T>(address);
        }

        public static async UniTask CreatePool(PoolableType poolableType, string name, Transform parent)
        {
            var address = $"{JoinSegment(PATH_SEGMENT, POOLABLE_PATH, poolableType.ToString(), name)}.{PREFAB_EXTENSION}";
            await ObjectPoolManager.Instance.CreatePool<PoolableObject>(address, parent);
        }

        private static string JoinSegment(char segment, params string[] parts)
        {
            if (parts == null || parts.Length == 0)
                return string.Empty;

            return string.Join(segment, parts);
        }
    }
}