using Cysharp.Threading.Tasks;
using Engine.Util.Extension;
using IdleProject.Battle.Character;
using IdleProject.Core.UI;
using UnityEngine;

using CharacterController = IdleProject.Battle.Character.CharacterController;

namespace IdleProject.Core
{
    public static class ResourcesLoader
    {
        private const string PrefabPath = "Prefab";
        private const string CharacterPath = "Character";
        private const string UIPath = "UI";

        private const string DataPath = "GameData";
        private const string CharacterDataPath = "CharacterData";

        private const char SplitSegement = '/';

        public async static UniTask<CharacterController> InstantiateCharacter(string name)
        {
            string address = $"{JoinWithSlash(PrefabPath, CharacterPath, name)}.prefab";
            var characterObj = await AddressableManager.Instance.Controller.InstantiateObject<GameObject>(address);
            return characterObj.GetComponent<CharacterController>();
        }

        public async static UniTask<T> InstantiateUI<T>(SceneType sceneType) where T : UIBase
        {
            string address = $"{JoinWithSlash(PrefabPath, UIPath, sceneType.ToString(), typeof(T).Name)}.prefab";
            var uiObj = await AddressableManager.Instance.Controller.InstantiateObject<GameObject>(address);
            return uiObj.GetComponent<T>();
        }

        public async static UniTask<CharacterData> LoadCharacterData(string name)
        {
            var data = await AddressableManager.Instance.Controller.LoadAssetAsync<CharacterData>($"{JoinWithSlash(DataPath, CharacterDataPath, name)}.asset");

            return data;
        }

        public static string JoinWithSlash(params string[] parts)
        {
            if (parts == null || parts.Length == 0)
                return string.Empty;

            return string.Join(SplitSegement, parts);
        }
    }
}
