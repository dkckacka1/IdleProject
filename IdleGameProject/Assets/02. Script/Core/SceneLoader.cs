using System;
using Cysharp.Threading.Tasks;
using Engine.Core.Time;
using IdleProject.Core.UI;
using Unity.VisualScripting;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

namespace IdleProject.Core
{
    public enum SceneType
    {
        Splash,
        Lobby,
        Battle,
        // Loading,
    }

    // 씬 이동 관리 클래스
    public class SceneLoader
    {
        private const string SCENE_ADDRESSABLE_PATH = "Scene";
        private const string SCENE_FILE_EXTENSION = "unity";

        private const float REQUIRED_SCENE_LOADING_TIME = 1f; // 최소 로딩 시간 보장

        public async UniTask<SceneController> LoadScene(SceneType sceneType)
        {
            SceneController sceneController = null;
            
            var loadingUI = UIManager.Instance.loadingUI;
            loadingUI.LoadingStart();

            var sceneInstance = await AddressableManager.Instance.Controller.LoadSceneAsync(
                $"{SCENE_ADDRESSABLE_PATH}/{sceneType}.{SCENE_FILE_EXTENSION}", LoadSceneMode.Single);
            await sceneInstance.ActivateAsync().ToUniTask();

            loadingUI.ShowLoadingPercent(0.3f);

            sceneController = UnityEngine.Object.FindAnyObjectByType<SceneController>();
            await sceneController.Initialize();

            loadingUI.ShowLoadingPercent(0.6f);

            await UniTask.WaitForSeconds(1f);
            loadingUI.ShowLoadingPercent(1f);

            return sceneController;
        }
    }
}