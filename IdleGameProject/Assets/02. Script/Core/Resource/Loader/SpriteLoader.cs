using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.U2D;


namespace IdleProject.Core.Resource
{
    public class SpriteLoader : AssetLoader<Sprite> 
    {
        private readonly Dictionary<string, SpriteAtlas> _atlasCacheDic = new();

        private const string SPRITEATLAS_ADDRESSABLE_LABEL_NAME = "SpriteAtlas";

        public override async UniTask LoadAsset()
        {
            var atlasList =
                await AddressableManager.Instance.Controller.LoadAssetsLabelAsync<SpriteAtlas>(
                    SPRITEATLAS_ADDRESSABLE_LABEL_NAME);

            foreach (var atlas in atlasList)
            {
                _atlasCacheDic.Add(atlas.name, atlas);
            }
        }

        public override Sprite GetAsset(string spriteName)
        {
            if (AssetCacheDic.ContainsKey(spriteName) is false)
            {
                var spriteAtlas = _atlasCacheDic[GetAtlasNameBySpriteName(spriteName)];
                var sprite = spriteAtlas.GetSprite(spriteName);
                if (sprite is not null)
                {
                    AssetCacheDic.Add(spriteName, sprite);
                }
                else
                {
                    throw new Exception($"{spriteName} is Invalid Key");
                }
            }

            return AssetCacheDic[spriteName];
        }

        private string GetAtlasNameBySpriteName(string spriteName)
        {
            var spriteTypeName = spriteName.Split('_')[0];

            return spriteTypeName switch
            {
                "Icon" => "IconSpriteAtlas",
                "UI" => "UISpriteAtlas",
                _ => ""
            };
        }
    }
}