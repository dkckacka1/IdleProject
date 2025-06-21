using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Data;
using IdleProject.Lobby.Character;
using IdleProject.Lobby.UI;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Lobby
{
    public class LobbyManager : SceneController
    {
        private LobbyUIController _lobbyUIController;

        private const string LOBBY_INIT_TASK = "LobbyInit";
        private const string MAIN_CHARACTER_INIT_TASK = "MainCharacterInit";

        [BoxGroup("LobbyMainCharacter"), SerializeField]
        private LobbyCharacter frontMiddleCharacter;
        
        [BoxGroup("LobbyMainCharacter"), SerializeField]
        private LobbyCharacter frontRightCharacter;
        
        [BoxGroup("LobbyMainCharacter"), SerializeField]
        private LobbyCharacter frontLeftCharacter;
        
        [BoxGroup("LobbyMainCharacter"), SerializeField]
        private LobbyCharacter rearRightCharacter;
        
        [BoxGroup("LobbyMainCharacter"), SerializeField]
        private LobbyCharacter rearLeftCharacter;
        
        public override async UniTask Initialize()
        {
            _lobbyUIController = UIManager.Instance.GetUIController<LobbyUIController>();

            TaskChecker.StartLoading(LOBBY_INIT_TASK, _lobbyUIController.Initialized);
            TaskChecker.StartLoading(LOBBY_INIT_TASK, SetMainCharacter);
            
            await UniTask.WaitUntil(() => TaskChecker.IsTasking(LOBBY_INIT_TASK) is false);
        }

        private async UniTask SetMainCharacter()
        {
            var formation = DataManager.Instance.DataController.userData.UserFormation;
            
            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK, () => SetCharacter(formation.frontLeftCharacterName, frontLeftCharacter));
            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK, () => SetCharacter(formation.frontMiddleCharacterName, frontMiddleCharacter));
            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK, () => SetCharacter(formation.frontRightCharacterName, frontRightCharacter));
            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK, () => SetCharacter(formation.rearLeftCharacterName, rearLeftCharacter));
            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK, () => SetCharacter(formation.rearRightCharacterName, rearRightCharacter));
            await UniTask.WaitUntil(() => TaskChecker.IsTasking(MAIN_CHARACTER_INIT_TASK) is false);
        }

        private async UniTask SetCharacter(string heroName, LobbyCharacter character)
        {
            if (string.IsNullOrEmpty(heroName))
                return;

            var data = DataManager.Instance.GetData<CharacterData>(heroName);

            var model = await ResourceLoader.InstantiateCharacterModel(data.addressValue.characterName,character.transform);
            character.SetModel(model);

            var animatorController = ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(data.addressValue.characterAnimationName);
            
            character.SetAnimation(animatorController);
        }
    }
}
