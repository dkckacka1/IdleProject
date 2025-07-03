using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.Resource
{
    public class AudioLoader : AssetLoader<AudioClip>
    {
        public AudioLoader(string assetLabelName) : base(assetLabelName) {}

        public override async UniTask LoadAsset()
        {
            var audioClipList = await AddressableManager.Instance.Controller.LoadAssetsLabelAsync<AudioClip>(AssetLabelName);

            foreach (var audioClip in audioClipList)
            {
                AssetCacheDic.Add(audioClip.name, audioClip);
            }
        }

        public override AudioClip GetAsset(string address)
        {
            return AssetCacheDic.GetValueOrDefault(address);
        }
    }
}