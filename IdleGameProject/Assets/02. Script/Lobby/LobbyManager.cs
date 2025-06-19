using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Engine.Util.Extension;
using IdleProject.Character;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Data;
using IdleProject.Lobby.UI;
using UnityEngine;

using CharacterController = IdleProject.Character.CharacterController;


namespace IdleProject.Lobby
{
    public class LobbyManager : SceneController
    {
        private LobbyUIController _lobbyUIController;

        [SerializeField]
        private List<LobbyCharacterController> lobbyCharacterList;
        
        private const string LOBBY_INIT_TASK = "LobbyInit";
        
        public override async UniTask Initialize()
        {
            _lobbyUIController = UIManager.Instance.GetUIController<LobbyUIController>();

            TaskChecker.StartLoading(LOBBY_INIT_TASK, _lobbyUIController.Initialized);
            TaskChecker.StartLoading(LOBBY_INIT_TASK, SetLobbyCharacters);
            
            await UniTask.WaitUntil(() => TaskChecker.IsTasking(LOBBY_INIT_TASK) is false);
        }

        private async UniTask SetLobbyCharacters()
        {
            var formation = DataManager.Instance.DataController.playerFormationInfo;
            await SetLobbyCharacter(formation.frontLeftCharacterName, lobbyCharacterList[0]);
            await SetLobbyCharacter(formation.frontMiddleCharacterName, lobbyCharacterList[1]);
            await SetLobbyCharacter(formation.frontRightCharacterName, lobbyCharacterList[2]);
            await SetLobbyCharacter(formation.rearLeftCharacterName, lobbyCharacterList[3]);
            await SetLobbyCharacter(formation.rearRightCharacterName, lobbyCharacterList[4]);
        }

        private async UniTask SetLobbyCharacter(string heroName, CharacterController controller)
        {
            if (string.IsNullOrEmpty(heroName))
                return;

            var data = DataManager.Instance.GetData<CharacterData>(heroName);
            var model = await ResourceLoader.InstantiateCharacterModel(data.addressValue.characterName, controller);
                model.SetLayerRecursively(LayerMask.NameToLayer("UIObject"));
            
            controller.SetModel(model);
            var animator = controller.GetComponent<Animator>();
            var animController =
                ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(data.addressValue.characterAnimationName);

            animator.runtimeAnimatorController = animController;
            animator.Rebind();
        }
    }
}
