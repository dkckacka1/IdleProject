using System;
using Cysharp.Threading.Tasks;
using Engine.Core.Addressable;
using IdleProject.Core;
using IdleProject.Core.ObjectPool;
using UnityEngine;
using Zenject;
using CharacterController = IdleProject.Battle.Character.CharacterController;
using IPoolable = IdleProject.Core.ObjectPool.IPoolable;

namespace IdleProject.Battle
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

    public class BattleResourceLoader
    {
        [Inject]
        private BattleManager _battleManager;
        
        private const string PREFAB_PATH = "Prefab";
        private const string POOLABLE_PATH = "Poolable";

        private const string DATA_PATH = "GameData";
        private const string CHARACTER_DATA_PATH = "CharacterData";

        private const char PATH_SEGMENT = '/';
        private const char SPRITE_NAME_SEGMENT = '_';

        public string GetCharacterControllerAddress(string name) => $"{JoinSegment(PATH_SEGMENT, PREFAB_PATH, nameof(PrefabType.Character), name)}.prefab";
        public async UniTask<CharacterController> InstantiateCharacter(string name)
        {
            var address = $"{JoinSegment(PATH_SEGMENT, PREFAB_PATH, nameof(PrefabType.Character), name)}.prefab";
            
            return await AddressableManager.Instance.InstantiateObject<CharacterController>(address, container: ProjectContext.Instance.Container);
        }
        
        public async UniTask<T> InstantiateUI<T>(SceneType sceneType, string name) where T : Component
        {
            var address =
                $"{JoinSegment(PATH_SEGMENT, PREFAB_PATH, nameof(PrefabType.UI), sceneType.ToString(), name)}.prefab";
            var uiObj = await AddressableManager.Instance.InstantiateObject<GameObject>(address, container: ProjectContext.Instance.Container);
            return uiObj.GetComponent<T>();
        }

        public async UniTask<CharacterData> LoadCharacterData(string name)
        {
            var address = $"{JoinSegment(PATH_SEGMENT, DATA_PATH, CHARACTER_DATA_PATH, name)}.asset";
            var data = await AddressableManager.Instance.LoadAssetAsync<CharacterData>(address);
            return data;
        }

        public T GetPoolableObject<T>(PoolableType poolableType, string name) where T : IPoolable
        {
            var address = $"{JoinSegment(PATH_SEGMENT, POOLABLE_PATH, poolableType.ToString(), name)}.prefab";
            return ObjectPoolManager.Instance.Get<T>(address, GetBattleTransformParent(poolableType));
        }

        public async UniTask CreatePool(PoolableType poolableType, string name)
        {
            var address = $"{JoinSegment(PATH_SEGMENT, POOLABLE_PATH, poolableType.ToString(), name)}.prefab";
            await ObjectPoolManager.Instance.CreatePool<PoolableObject>(address,
                GetBattleTransformParent(poolableType));
        }

        public async UniTask CreatePool(PoolableType poolableType, string name, Transform parent)
        {
            var address = $"{JoinSegment(PATH_SEGMENT, POOLABLE_PATH, poolableType.ToString(), name)}.prefab";
            await ObjectPoolManager.Instance.CreatePool<PoolableObject>(address, parent);
        }

        public async UniTask<Sprite> GetIcon(IconType iconType, string name, string type)
        {
            var spriteName = JoinSegment(SPRITE_NAME_SEGMENT, iconType.ToString(), name, type, "Icon");
            var address = $"{JoinSegment(PATH_SEGMENT, "Icon", spriteName)}.png";

            var sprite = await AddressableManager.Instance.LoadAssetAsync<Sprite>(address);

            return sprite;
        }

        private string JoinSegment(char segment, params string[] parts)
        {
            if (parts == null || parts.Length == 0)
                return string.Empty;

            return string.Join(segment, parts);
        }

        private Transform GetBattleTransformParent(PoolableType poolableType)
        {
            Transform parent = null;
            
            switch (poolableType)
            {
                case PoolableType.UI:
                    break;
                case PoolableType.Effect:
                    parent = _battleManager.EffectTransformOffset;
                    break;
                case PoolableType.Projectile:
                    parent = _battleManager.ProjectileTransformOffset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(poolableType), poolableType, null);
            }

            return parent;
        }
    }
}