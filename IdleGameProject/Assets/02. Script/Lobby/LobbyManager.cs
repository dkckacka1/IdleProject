using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Data;
using IdleProject.Data.StaticData;
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

            await TaskChecker.WaitTasking(LOBBY_INIT_TASK);
        }

        private async UniTask SetMainCharacter()
        {
            var formation = DataManager.Instance.DataController.Player.PlayerFormation;

            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK,
                () => SetCharacter(formation.frontLeftPositionInfo, frontLeftCharacter));
            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK,
                () => SetCharacter(formation.frontMiddlePositionInfo, frontMiddleCharacter));
            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK,
                () => SetCharacter(formation.frontRightPositionInfo, frontRightCharacter));
            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK,
                () => SetCharacter(formation.rearLeftPositionInfo, rearLeftCharacter));
            TaskChecker.StartLoading(MAIN_CHARACTER_INIT_TASK,
                () => SetCharacter(formation.rearRightPositionInfo, rearRightCharacter));
            await TaskChecker.WaitTasking(MAIN_CHARACTER_INIT_TASK);
        }

        private async UniTask SetCharacter(PositionInfo positionInfo, LobbyCharacter character)
        {
            if (string.IsNullOrEmpty(positionInfo.characterName))
                return;

            var data = DataManager.Instance.GetData<StaticCharacterData>(positionInfo.characterName);
            
            var modelObject = ResourceManager.Instance.GetPrefab(ResourceManager.GamePrefab,
                $"Model_{data.addressValue.characterName}");
            character.SetModel(Instantiate(modelObject,character.transform));

            var animatorController =
                ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(data.addressValue.characterAnimationName);

            character.SetAnimation(animatorController).Forget();
        }
    }
}