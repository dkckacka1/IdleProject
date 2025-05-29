using Cysharp.Threading.Tasks;
using Engine.Core.Addressable;
using IdleProject.Battle;
using IdleProject.Core.ObjectPool;
using IdleProject.Core.UI;
using UnityEngine;
using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Core
{
    public enum PrefabType
    {
        Character,
        UI
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

    public static class ResourcesLoader
    {
        private const string PREFAB_PATH = "Prefab";
        private const string POOLABLE_PATH = "Poolable";

        private const string DATA_PATH = "GameData";
        private const string CHARACTER_DATA_PATH = "CharacterData";

        private const char PATH_SEGEMENT = '/';
        private const char SPRITE_NAME_SEGEMENT = '_';

        public static async UniTask<CharacterController> InstantiateCharacter(string name)
        {
            var address = $"{JoinSegement(PATH_SEGEMENT, PREFAB_PATH, nameof(PrefabType.Character), name)}.prefab";
            return await AddressableManager.Instance.InstantiateObject<CharacterController>(address);
        }

        public static async UniTask<T> InstantiateUI<T>(SceneType sceneType, string name) where T : Component
        {
            var address =
                $"{JoinSegement(PATH_SEGEMENT, PREFAB_PATH, nameof(PrefabType.UI), sceneType.ToString(), name)}.prefab";
            var uiObj = await AddressableManager.Instance.InstantiateObject<GameObject>(address);
            return uiObj.GetComponent<T>();
        }

        public static async UniTask<CharacterData> LoadCharacterData(string name)
        {
            var address = $"{JoinSegement(PATH_SEGEMENT, DATA_PATH, CHARACTER_DATA_PATH, name)}.asset";
            var data = await AddressableManager.Instance.LoadAssetAsync<CharacterData>(address);
            return data;
        }

        public static T GetPoolableObject<T>(PoolableType poolableType, string name) where T : IPoolable
        {
            var address = $"{JoinSegement(PATH_SEGEMENT, POOLABLE_PATH, poolableType.ToString(), name)}.prefab";
            return ObjectPoolManager.Instance.Get<T>(address, GetBattleTransformParent(poolableType));
        }

        public static async UniTask CreatePool(PoolableType poolableType, string name)
        {
            var address = $"{JoinSegement(PATH_SEGEMENT, POOLABLE_PATH, poolableType.ToString(), name)}.prefab";
            await ObjectPoolManager.Instance.CreatePool<PoolableObject>(address,
                GetBattleTransformParent(poolableType));
        }

        public static async UniTask CreatePool(PoolableType poolableType, string name, Transform parent)
        {
            var address = $"{JoinSegement(PATH_SEGEMENT, POOLABLE_PATH, poolableType.ToString(), name)}.prefab";
            await ObjectPoolManager.Instance.CreatePool<PoolableObject>(address, parent);
        }

        public static async UniTask<Sprite> GetIcon(IconType iconType, string name, string type)
        {
            var spriteName = JoinSegement(SPRITE_NAME_SEGEMENT, iconType.ToString(), name, type, "Icon");
            var address = $"{JoinSegement(PATH_SEGEMENT, "Icon", spriteName)}.png";

            var sprite = await AddressableManager.Instance.LoadAssetAsync<Sprite>(address);

            return sprite;
        }

        private static string JoinSegement(char segement, params string[] parts)
        {
            if (parts == null || parts.Length == 0)
                return string.Empty;

            return string.Join(segement, parts);
        }

        private static Transform GetBattleTransformParent(PoolableType poolableType)
        {
            Transform parent = null;

            switch (poolableType)
            {
                case PoolableType.UI:
                    break;
                case PoolableType.Effect:
                    parent = BattleManager.Instance.effectParent;
                    break;
                case PoolableType.Projectile:
                    parent = BattleManager.Instance.projectileParent;
                    break;
            }

            return parent;
        }
    }
}