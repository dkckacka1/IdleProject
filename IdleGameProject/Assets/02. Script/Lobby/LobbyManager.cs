using System;
using Cysharp.Threading.Tasks;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Loading;
using IdleProject.Core.Resource;
using IdleProject.Core.Sound;
using IdleProject.Data.StaticData;
using IdleProject.Lobby.Character;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Lobby
{
    public class LobbyManager : SceneController
    {
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

        private bool _isTestToggle;
        
        public override async UniTask Initialize()
        {
            TaskChecker.StartLoading(LOBBY_INIT_TASK, SetMainCharacter);

            await TaskChecker.WaitTasking(LOBBY_INIT_TASK);
        }

        private void Start()
        {
            SoundManager.Instance.PlayBGM(DataManager.Instance.ConstData.LobbySceneBgmName);
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
                $"Model_{data.Index}");
            character.SetModel(Instantiate(modelObject,character.transform));

            var animatorController =
                ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(data.characterAnimationName);

            character.SetAnimation(animatorController).Forget();
        }
        
        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 50, 50), "TestToggle"))
            {
                _isTestToggle = !_isTestToggle;
            }

            if (_isTestToggle)
            {
                if (GUI.Button(new Rect(10, 60, 100, 100), "AddExpPotion"))
                {
                    DataManager.Instance.DataController.Player.AddConsumableItem("ExpPotion05", 100);
                }
            }
        }
    }
}