using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.Resource
{
    public class AnimationControllerLoader : AssetLoader<RuntimeAnimatorController>
    {
        private const string ANIMATION_CONTROLLER_ADDRESSABLE_L_ABEL_NAME = "Animation";

        public override async UniTask LoadAsset()
        {
            var atlasList =
                await AddressableManager.Instance.Controller.LoadAssetsLabelAsync<RuntimeAnimatorController>(
                    ANIMATION_CONTROLLER_ADDRESSABLE_L_ABEL_NAME);

            foreach (var atlas in atlasList)
            {
                AssetCacheDic.Add(atlas.name, atlas);
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