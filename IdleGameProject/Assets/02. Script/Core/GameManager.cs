using Engine.Util;
using Sirenix.OdinInspector;

namespace IdleProject.Core
{
    // 게임 매니저 클래스
    public class GameManager : SingletonMonoBehaviour<GameManager>
    {
        private SceneLoader _sceneLoader;
        
        private SceneController _currentSceneController;

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

        public void SetSceneManager(SceneController sceneController)
        {
            _currentSceneController = sceneController;
        }

        [Button]
        private async  void LoadSceneTest()
        {
            await _sceneLoader.LoadScene(SceneType.Battle, true);
        }
    }
}
