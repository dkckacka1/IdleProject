using Cysharp.Threading.Tasks;
using IdleProject.Core.ObjectPool;
using UnityEngine;

namespace IdleProject.Core
{
    public static class ResourceLoader
    {
        private const string PREFAB_PATH = "Prefab";
        private const string POOLABLE_PATH = "Poolable";

        private const string PREFAB_EXTENSION = "prefab";

        private const char PATH_SEGMENT = '/';

        public static async UniTask<T> InstantiateUI<T>(SceneType sceneType, string name) where T : Component
        {
            var address =
                $"{JoinSegment(PATH_SEGMENT, PREFAB_PATH, nameof(ResourceType.UI), sceneType.ToString(), name)}.{PREFAB_EXTENSION}";
            var uiObj = await AddressableManager.Instance.Controller.InstantiateObject<GameObject>(address);
            return uiObj.GetComponent<T>();
        }

        private static string JoinSegment(char segment, params string[] parts)
        {
            if (parts == null || parts.Length == 0)
                return string.Empty;

            return string.Join(segment, parts);
        }
    }
}