using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Engine.Util;
using IdleProject.Core.Loading;

namespace IdleProject.Core.Resource
{
    public class ResourceManager : SingletonMonoBehaviour<ResourceManager>
    {
        public SpriteLoader SpriteLoader = new SpriteLoader();

        private const string ASSET_LOADING_TASK = "AssetLoading";
        
        protected override void Initialized()
        {
            base.Initialized();
        }

        public async UniTask LoadAssets()
        {
            TaskChecker.StartLoading(ASSET_LOADING_TASK, SpriteLoader.LoadAsset);

            await UniTask.WaitUntil(() => TaskChecker.IsTasking(ASSET_LOADING_TASK) is false);
        }
    }
}