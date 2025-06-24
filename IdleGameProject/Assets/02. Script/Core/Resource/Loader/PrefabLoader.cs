using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.Resource
{
    public class PrefabLoader : AssetLoader<GameObject>
    {
        private string _labelName;
        
        public PrefabLoader(string labelName)
        {
            _labelName = labelName;
        }
        
        public override async UniTask LoadAsset()
        {
            var atlasList = await AddressableManager.Instance.Controller.LoadAssetsLabelAsync<GameObject>(_labelName);

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