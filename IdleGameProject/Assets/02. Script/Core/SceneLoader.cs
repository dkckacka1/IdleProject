using Cysharp.Threading.Tasks;
using IdleProject.Core.UI;
using UnityEngine.SceneManagement;

namespace IdleProject.Core
{

    // 씬 이동 관리 클래스
    public class SceneLoader
    {
        private const string SCENE_ADDRESSABLE_PATH = "Scene";
        private const string SCENE_FILE_EXTENSION = "unity";
        
        public async UniTask<SceneController> LoadScene(SceneType sceneType, bool isLoadingUI = true)
        {
            SceneController sceneController = null;

            if (isLoadingUI)
            {
                var loadingUI = UIManager.Instance.loadingUI;
                loadingUI.LoadingStart();

                var sceneInstance = await AddressableManager.Instance.Controller.LoadSceneAsync(
                    $"{SCENE_ADDRESSABLE_PATH}/{sceneType}.{SCENE_FILE_EXTENSION}", LoadSceneMode.Single);
                await sceneInstance.ActivateAsync().ToUniTask();

                loadingUI.ShowLoadingPercent(0.3f);

                sceneController = UnityEngine.Object.FindAnyObjectByType<SceneController>();
                await sceneController.Initialize();
                loadingUI.ShowLoadingPercent(0.6f);

                UIManager.Instance.InitializedUI();
                loadingUI.ShowLoadingPercent(1f);
            }
            else
            {
                var sceneInstance = await AddressableManager.Instance.Controller.LoadSceneAsync($"{SCENE_ADDRESSABLE_PATH}/{sceneType}.{SCENE_FILE_EXTENSION}", LoadSceneMode.Single);
                await sceneInstance.ActivateAsync().ToUniTask();
                
                sceneController = UnityEngine.Object.FindAnyObjectByType<SceneController>();
                await sceneController.Initialize();
                UIManager.Instance.InitializedUI();
            }

            return sceneController;
        }
    }
}