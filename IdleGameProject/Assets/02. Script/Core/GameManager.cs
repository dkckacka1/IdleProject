using System;
using Cysharp.Threading.Tasks;
using Engine.Util;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using IdleProject.Core.Resource;
using Sirenix.OdinInspector;
using UnityEngine.Events;

namespace IdleProject.Core
{
    // 게임 매니저 클래스
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private SceneLoader _sceneLoader;

        private SceneController _currentSceneController;

        private const string GAME_INIT_TASK = "GameInit";

        public static T GetCurrentSceneManager<T>() where T : SceneController => Instance._currentSceneController as T;

        public static void SetCurrentSceneManager(SceneController controller)
        {
            Instance._currentSceneController = controller;
        }

        protected override void Initialized()
        {
            base.Initialized();

            _sceneLoader = new SceneLoader();
        }

        public async UniTask LoadScene(SceneType sceneType, UnityAction sceneLoadComplete = null)
        {
            await _sceneLoader.LoadScene(sceneType);
            sceneLoadComplete?.Invoke();
        }
        
        private void Start()
        {
            TaskChecker.StartLoading(GAME_INIT_TASK, DataManager.Instance.LoadData);
            TaskChecker.StartLoading(GAME_INIT_TASK, ResourceManager.Instance.LoadAssets);
            TaskChecker.AddOnCompleteCallback(GAME_INIT_TASK,
                () => { LoadScene(SceneType.Lobby).Forget(); });
        }
    }
}