using Cysharp.Threading.Tasks;
using IdleProject.Core.GameData;
using IdleProject.Core.Sound;
using IdleProject.Core.UI;
using UnityEngine;

namespace IdleProject.Lobby.UI
{
    public class LobbyUIController : UIController
    {
        [SerializeField] private string panelOpenSoundName;
        
        private readonly PanelRadioGroup _lobbyPanelRadioGroup = new();
        
        public override async UniTask Initialized()
        {
            var characterPanel = UIManager.Instance.GetUI<CharacterPanel.CharacterPanel>();
            var stagePanel = UIManager.Instance.GetUI<StagePanel.StagePanel>();
            
            _lobbyPanelRadioGroup.PublishPanel(characterPanel);
            _lobbyPanelRadioGroup.PublishPanel(stagePanel);

            UIManager.Instance.GetUI<UIButton>("EquipmentButton").Button.onClick.AddListener(OpenEquipmentPanel);
            UIManager.Instance.GetUI<UIButton>("DungeonButton").Button.onClick.AddListener(GoToDungeon);
            UIManager.Instance.GetUI<UIButton>("SettingButton").Button.onClick.AddListener(OpenSettingPopup);
            
            characterPanel.ClosePanel();
            stagePanel.ClosePanel();
            UIManager.Instance.GetUI<IdleProject.SettingPopup>().ClosePopup();
            
            UIManager.Instance.GetUI<PlayerInfoUI>().SetPlayerInfo(DataManager.Instance.DataController.Player.PlayerInfo);
        }

        private void OpenSettingPopup()
        {
            UIManager.Instance.GetUI<IdleProject.SettingPopup>().OpenPopup();
        }

        private void GoToDungeon()
        {
            UIManager.Instance.GetUI<StagePanel.StagePanel>().OpenPanel();
            SoundManager.Instance.PlaySfx(panelOpenSoundName);
        }

        private void OpenEquipmentPanel()
        {
            UIManager.Instance.GetUI<CharacterPanel.CharacterPanel>().OpenPanel();
        }
    }
}
