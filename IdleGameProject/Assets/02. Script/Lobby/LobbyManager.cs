using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Core.Loading;
using IdleProject.Core.UI;
using IdleProject.Lobby.UI;

namespace IdleProject.Lobby
{
    public class LobbyManager : SceneController
    {
        private LobbyUIController _lobbyUIController;

        private const string LOBBY_INIT_TASK = "LobbyInit";
        
        public override async UniTask Initialize()
        {
            _lobbyUIController = UIManager.Instance.GetUIController<LobbyUIController>();

            TaskChecker.StartLoading(LOBBY_INIT_TASK, _lobbyUIController.Initialized);
            
            await UniTask.WaitUntil(() => TaskChecker.IsTasking(LOBBY_INIT_TASK) is false);
        }
    }
}
