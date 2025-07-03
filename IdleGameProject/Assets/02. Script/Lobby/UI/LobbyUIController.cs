using Cysharp.Threading.Tasks;
using IdleProject.Core.UI;

namespace IdleProject.Lobby.UI
{
    public class LobbyUIController : UIController
    {
        private readonly PanelRadioGroup _lobbyPanelRadioGroup = new();
        
        public override async UniTask Initialized()
        {
            var characterPanel = UIManager.Instance.GetUI<CharacterPanel.CharacterPanel>();
            var stagePanel = UIManager.Instance.GetUI<StagePanel.StagePanel>();
            
            _lobbyPanelRadioGroup.PublishPanel(characterPanel);
            _lobbyPanelRadioGroup.PublishPanel(stagePanel);

            UIManager.Instance.GetUI<UIButton>("EquipmentButton").Button.onClick.AddListener(OpenEquipmentPanel);
            UIManager.Instance.GetUI<UIButton>("DungeonButton").Button.onClick.AddListener(GoToDungeon);
            
            characterPanel.ClosePanel();
            stagePanel.ClosePanel();
        }

        private void GoToDungeon()
        {
            UIManager.Instance.GetUI<StagePanel.StagePanel>().OpenPanel();
        }

        private void OpenEquipmentPanel()
        {
            UIManager.Instance.GetUI<CharacterPanel.CharacterPanel>().OpenPanel();
        }
    }
}
