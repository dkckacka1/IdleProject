using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Engine.Util;
using IdleProject.Core.Loading;
using Sirenix.OdinInspector;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IdleProject.Core.Resource
{
    public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
    {
        [ShowInInspector]
        private readonly Dictionary<Type, IAssetLoader<Object>> _assetLoaderDic = new();
        [ShowInInspector]
        private readonly Dictionary<string, IAssetLoader<GameObject>> _prefabLoaderDic = new();

        private const string ASSET_LOADING_TASK = "AssetLoading";

        public const string GamePrefab = "GamePrefab";
        public const string UIPrefab = "UIPrefab";
        public const string PoolableObject = "PoolableObject";
        private const string SPRITE_ATLAS_LABEL_NAME = "SpriteAtlas";
        private const string ANIMATION_LABEL_NAME = "Animation";
        private const string AUDIO_CLIP_LABEL_NAME = "AudioClip";
        
        protected override void Initialized()
        {
            base.Initialized();
            
            _assetLoaderDic.Add(typeof(Sprite), new SpriteLoader(SPRITE_ATLAS_LABEL_NAME));
            _assetLoaderDic.Add(typeof(RuntimeAnimatorController), new AnimationControllerLoader(ANIMATION_LABEL_NAME));
            _assetLoaderDic.Add(typeof(AudioClip), new AudioLoader(AUDIO_CLIP_LABEL_NAME));
            _prefabLoaderDic.Add(GamePrefab, new PrefabLoader(GamePrefab));
            _prefabLoaderDic.Add(UIPrefab, new PrefabLoader(UIPrefab));
            _prefabLoaderDic.Add(PoolableObject, new PrefabLoader(PoolableObject));
        }

        public T GetAsset<T>(string assetName) where T : Object
        {
            if (_assetLoaderDic.ContainsKey(typeof(T)))
            {
                return _assetLoaderDic[typeof(T)].GetAsset(assetName) as T;
            }

            return null;
        }

        public GameObject GetPrefab(string prefabType, string prefabName)
        {
            if (_prefabLoaderDic.ContainsKey(prefabType))
            {
                return _prefabLoaderDic[prefabType].GetAsset(prefabName);
            }

            return null;
        }
        
        public async UniTask LoadAssets()
        {
            foreach (var loader in _assetLoaderDic.Values)
            {
                TaskChecker.StartLoading(ASSET_LOADING_TASK, loader.LoadAsset);
            }

            foreach (var prefabLoader in _prefabLoaderDic)
            {
                TaskChecker.StartLoading(ASSET_LOADING_TASK, prefabLoader.Value.LoadAsset);
            }

            await TaskChecker.WaitTasking(ASSET_LOADING_TASK);
        }
    }
}