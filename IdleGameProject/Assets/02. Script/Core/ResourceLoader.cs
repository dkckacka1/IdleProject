using System;
using Cysharp.Threading.Tasks;
using Engine.Core.Addressable;
using IdleProject.Battle;
using IdleProject.Core.ObjectPool;
using IdleProject.Data;
using UnityEngine;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Core
{
    public enum ResourceType
    {
        Character,
        UI,
        Animation
    }

    public enum PoolableType
    {
        UI,
        Effect,
        Projectile,
    }

    public enum IconType
    {
        Character,
    }

    public static class ResourceLoader
    {
        private const string PREFAB_PATH = "Prefab";
        private const string POOLABLE_PATH = "Poolable";
        private const string MODEL_PATH = "Model";
        private const string ANIMATION_PATH = "Animation";

        private const string PREFAB_EXTENSION = "prefab";
        private const string ANIMATION_EXTENSION = "overrideController";

        private const char PATH_SEGMENT = '/';
        private const char SPRITE_NAME_SEGMENT = '_';

        
        public static async UniTask<CharacterController> InstantiateCharacter(string name)
        {
            var address = $"{JoinSegment(PATH_SEGMENT, PREFAB_PATH, nameof(ResourceType.Character), name)}.{PREFAB_EXTENSION}";
            return await AddressableManager.Instance.Controller.InstantiateObject<CharacterController>(address);
        }

        public static async UniTask<GameObject> InstantiateCharacterModel(string characterName, CharacterController controller)
        {
            var prefabName = $"{MODEL_PATH}_{characterName}";
            var address = $"{JoinSegment(PATH_SEGMENT, PREFAB_PATH, nameof(ResourceType.Character), MODEL_PATH, prefabName)}.{PREFAB_EXTENSION}";
            var model = await AddressableManager.Instance.Controller.InstantiateObject<GameObject>(address, parent: controller.transform);

            return model;
        }

        public static async UniTask<RuntimeAnimatorController> GetAnimation(string animationName)
        {
            var address = $"{JoinSegment(PATH_SEGMENT, ANIMATION_PATH, animationName)}.{ANIMATION_EXTENSION}";
            return await AddressableManager.Instance.Controller.LoadAssetAsync<RuntimeAnimatorController>(address);
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

        public static async UniTask CreatePool(PoolableType poolableType, string name)
        {
            var address = $"{JoinSegment(PATH_SEGMENT, POOLABLE_PATH, poolableType.ToString(), name)}.{PREFAB_EXTENSION}";
            await ObjectPoolManager.Instance.CreatePool<PoolableObject>(address,
                GetBattleTransformParent(poolableType));
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

        private static Transform GetBattleTransformParent(PoolableType poolableType)
        {
            Transform parent = null;

            switch (poolableType)
            {
                case PoolableType.UI:
                    break;
                case PoolableType.Effect:
                    parent = GameManager.GetCurrentSceneManager<BattleManager>().effectParent;
                    break;
                case PoolableType.Projectile:
                    parent = GameManager.GetCurrentSceneManager<BattleManager>().projectileParent;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(poolableType), poolableType, null);
            }

            return parent;
        }
    }
}