using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.Resource
{
    public class PrefabLoader : AssetLoader<GameObject>
    {
        public PrefabLoader(string assetLabelName) : base(assetLabelName) {}

        public override async UniTask LoadAsset()
        {
            var atlasList = await AddressableManager.Instance.Controller.LoadAssetsLabelAsync<GameObject>(AssetLabelName);

            foreach (var atlas in atlasList)
            {
                AssetCacheDic.Add(atlas.name, atlas);
            }
        }

        public override GameObject GetAsset(string address)
        {
            if (AssetCacheDic.TryGetValue(address, out var prefab) is false)
            {
                Debug.LogError($"{address} is Invalid Key");
            }

            return prefab;
        }
    }
}