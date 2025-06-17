using System;
using Cysharp.Threading.Tasks;
using Engine.Util;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using Sirenix.OdinInspector;

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

        public void LoadScene(SceneType sceneType)
        {
            _sceneLoader.LoadScene(SceneType.Battle).Forget();
        }

        private void Start()
        {
            TaskChecker.StartLoading(GAME_INIT_TASK, DataManager.Instance.LoadData);
            TaskChecker.StartLoading(GAME_INIT_TASK, () => UniTask.WaitForSeconds(1f));
            TaskChecker.AddOnCompleteCallback(GAME_INIT_TASK,
                () => { LoadScene(SceneType.Battle); });
        }
    }
}