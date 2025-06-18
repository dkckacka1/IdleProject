using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Engine.Util;
using IdleProject.Core.Loading;
using UnityEngine;
using Object = UnityEngine.Object;

namespace IdleProject.Core.Resource
{
    public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
    {
        private readonly Dictionary<Type, IAssetLoader<Object>> _assetLoaderDic = new();

        private const string ASSET_LOADING_TASK = "AssetLoading";
        
        protected override void Initialized()
        {
            base.Initialized();
            
            _assetLoaderDic.Add(typeof(Sprite), new SpriteLoader());
            _assetLoaderDic.Add(typeof(RuntimeAnimatorController), new AnimationControllerLoader());
        }


        public T GetAsset<T>(string assetName) where T : Object
        {
            if (_assetLoaderDic.ContainsKey(typeof(T)))
            {
                return _assetLoaderDic[typeof(T)].GetAsset(assetName) as T;
            }

            return null;
        }
        
        
        public async UniTask LoadAssets()
        {
            foreach (var loader in _assetLoaderDic.Values)
            {
                TaskChecker.StartLoading(ASSET_LOADING_TASK, loader.LoadAsset);
            }

            await UniTask.WaitUntil(() => TaskChecker.IsTasking(ASSET_LOADING_TASK) is false);
        }
    }
}