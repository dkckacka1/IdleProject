using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.Resource
{
    public class AnimationControllerLoader : AssetLoader<RuntimeAnimatorController>
    {
        public AnimationControllerLoader(string assetLabelName) : base(assetLabelName) {}

        public override async UniTask LoadAsset()
        {
            var animationList =
                await AddressableManager.Instance.Controller.LoadAssetsLabelAsync<RuntimeAnimatorController>(AssetLabelName);

            foreach (var animation in animationList)
            {
                AssetCacheDic.Add(animation.name, animation);
            }
        }

        public override RuntimeAnimatorController GetAsset(string animationName)
        {
            if (AssetCacheDic.TryGetValue(animationName, out var runtimeAnimatorController) is false)
            {
                throw new Exception($"{animationName} is Invalid Key");
            }

            return runtimeAnimatorController;
        }
    }
}