using Cysharp.Threading.Tasks;
using IdleProject.Core;

namespace IdleProject.Lobby
{
    public class LobbyManager : SceneController
    {
        public override async UniTask Initialize()
        {
            await UniTask.WaitForSeconds(1f);
        }
    }
}
