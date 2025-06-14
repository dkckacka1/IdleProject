using System;
using Cysharp.Threading.Tasks;
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
        Loading,
    }

    // 씬 이동 관리 클래스
    public class SceneLoader
    {
        private const string SCENE_ADDRESSABLE_PATH = "Scene";
        private const string SCENE_FILE_EXTENSION = "unity";

        public async UniTask<SceneController> LoadScene(SceneType sceneType, bool isLoading)
        {
            SceneController sceneController = null;

            var loadingUI = UIManager.Instance.loadingUI;
            if (isLoading)
                loadingUI.LoadingStart();

            var sceneInstance = await AddressableManager.Instance.Controller.LoadSceneAsync(
                $"{SCENE_ADDRESSABLE_PATH}/{sceneType}.{SCENE_FILE_EXTENSION}", LoadSceneMode.Single);
            await sceneInstance.ActivateAsync().ToUniTask();

            if (isLoading)
                loadingUI.ShowLoadingPercent(30f);

            sceneController = UnityEngine.Object.FindAnyObjectByType<SceneController>();
            sceneController.SceneInitialize();
            
            if (isLoading)
                loadingUI.ShowLoadingPercent(60f);

            if (isLoading)
                await UniTask.WaitForSeconds(1f);

            if (isLoading)
                loadingUI.LoadingEnd();


            return sceneController;
        }
    }
}