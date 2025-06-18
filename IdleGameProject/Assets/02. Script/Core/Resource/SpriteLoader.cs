using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;


namespace IdleProject.Core.Resource
{
    public class SpriteLoader : IAssetLoader<Sprite>
    {
        private readonly Dictionary<string, SpriteAtlas> _atlasCacheDic = new();
        private readonly Dictionary<string, Sprite> _spriteCacheDic = new(); // 한번 찾은 이미지는 빠른 찾기를 위해 추가로 저장 

        private const string SPRITEATLAS_ADDRESSABLE_LABEL_NAME = "SpriteAtlas";

        public async UniTask LoadAsset()
        {
            var atlasList =
                await AddressableManager.Instance.Controller.LoadAssetsLabelAsync<SpriteAtlas>(
                    SPRITEATLAS_ADDRESSABLE_LABEL_NAME);

            foreach (var atlas in atlasList)
            {
                _atlasCacheDic.Add(atlas.name, atlas);
            }
        }

        public Sprite GetAsset(string spriteName)
        {
            if (_spriteCacheDic.ContainsKey(spriteName) is false)
            {
                var spriteAtlas = _atlasCacheDic[GetAtlasNameBySpriteName(spriteName)];
                var sprite = spriteAtlas.GetSprite(spriteName);
                if (sprite is not null)
                {
                    _spriteCacheDic.Add(spriteName, sprite);
                }
                else
                {
                    throw new Exception($"{spriteName} is Invalid Key");
                }
            }

            return _spriteCacheDic[spriteName];
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