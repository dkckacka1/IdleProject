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

    public static class ResourcesLoader
    {
        private const string PrefabPath = "Prefab";
        private const string PoolablePath = "Poolable";

        private const string DataPath = "GameData";
        private const string CharacterDataPath = "CharacterData";

        private const char SplitSegement = '/';

        public async static UniTask<CharacterController> InstantiateCharacter(string name)
        {
            string address = $"{JoinWithSlash(PrefabPath, PrefabType.Character.ToString(), name)}.prefab";
            return await AddressableManager.Instance.InstantiateObject<CharacterController>(address);
        }

        public async static UniTask<T> InstantiateUI<T>(SceneType sceneType) where T : UIBase
        {
            string address = $"{JoinWithSlash(PrefabPath, PrefabType.UI.ToString(), sceneType.ToString(), typeof(T).Name)}.prefab";
            var uiObj = await AddressableManager.Instance.InstantiateObject<UIBase>(address);
            return uiObj.GetComponent<T>();
        }

        public async static UniTask<CharacterData> LoadCharacterData(string name)
        {
            var address = $"{JoinWithSlash(DataPath, CharacterDataPath, name)}.asset";
            var data = await AddressableManager.Instance.LoadAssetAsync<CharacterData>(address);
            return data;
        }

        public static T GetPoolableObject<T>(PoolableType poolableType, string name) where T : IPoolable
        {
            var address = $"{JoinWithSlash(PoolablePath, poolableType.ToString(), name)}.prefab";
            return ObjectPoolManager.Instance.Get<T>(address, GetBattleTransformParent(poolableType));
        }

        public async static UniTask CreatePool(PoolableType poolableType, string name)
        {
            var address = $"{JoinWithSlash(PoolablePath, poolableType.ToString(), name)}.prefab";
            await ObjectPoolManager.Instance.CreatePool<PoolableObject>(address, GetBattleTransformParent(poolableType));
        }

        private static string JoinWithSlash(params string[] parts)
        {
            if (parts == null || parts.Length == 0)
                return string.Empty;

            return string.Join(SplitSegement, parts);
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
