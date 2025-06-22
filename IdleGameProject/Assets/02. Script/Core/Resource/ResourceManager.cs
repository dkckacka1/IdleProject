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

        public const string CharacterModelLabelName = "CharacterModel";
        
        protected override void Initialized()
        {
            base.Initialized();
            
            _assetLoaderDic.Add(typeof(Sprite), new SpriteLoader());
            _assetLoaderDic.Add(typeof(RuntimeAnimatorController), new AnimationControllerLoader());
            _prefabLoaderDic.Add(CharacterModelLabelName, new PrefabLoader(CharacterModelLabelName));
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