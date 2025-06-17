using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;


namespace IdleProject.Core.Resource
{
    public class SpriteLoader : AssetLoader<Sprite>
    {
        private readonly Dictionary<string, SpriteAtlas> _atlasCacheDic = new Dictionary<string, SpriteAtlas>();
        private Dictionary<(string atlasKey, string spriteName), Sprite> _spriteCache = new();

        private const string SPRITEATLAS_ADDRESSABLE_LABEL_NAME = "SpriteAtlas";
        
        public override async UniTask LoadAsset()
        {
            var list = await AddressableManager.Instance.Controller.LoadAssetsLabelAsync<SpriteAtlas>(SPRITEATLAS_ADDRESSABLE_LABEL_NAME);

            foreach (var atlas in list)
            {
                Debug.Log(atlas.name);
            }
        }

        public override Sprite GetAsset(string address)
        {
            return null;
        }
    }
}
