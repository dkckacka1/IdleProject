using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Loading;

namespace IdleProject.Title.UI
{
    public class TitleUIController : UIController
    {
        private LoadingRotateUI _loadingUI;
        private UIButton _gameStartButton;
        
        public override async UniTask Initialized()
        {
            _loadingUI = UIManager.Instance.GetUI<LoadingRotateUI>("GameLoadingUI");
            _gameStartButton = UIManager.Instance.GetUI<UIButton>("GameStartButton");
            _gameStartButton.Button.onClick.AddListener(GoToLobbyScene);
        }

        public async UniTask Loading(UniTask task)
        {
            _gameStartButton.gameObject.SetActive(false);
            await _loadingUI.StartLoading(task);
            _gameStartButton.gameObject.SetActive(true);
        }
        
        private void GoToLobbyScene()
        {
            GameManager.Instance.LoadScene(SceneType.Lobby).Forget();
        }
    }
}