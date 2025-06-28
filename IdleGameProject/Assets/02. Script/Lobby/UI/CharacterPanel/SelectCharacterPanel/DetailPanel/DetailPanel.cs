using IdleProject.Core.UI;

namespace IdleProject.Lobby.UI.CharacterPanel
{
    public class DetailPanel : UIPanel
    {
        private PanelRadioGroup _panelRadioGroup;
        
        public override void Initialized()
        {
            _panelRadioGroup = new PanelRadioGroup();
            _panelRadioGroup.PublishPanel(UIManager.Instance.GetUI<SelectEquipmentItemPanel>());
            _panelRadioGroup.PublishPanel(UIManager.Instance.GetUI<CharacterStatPanel>());
            _panelRadioGroup.PublishPanel(UIManager.Instance.GetUI<CharacterLevelUpPanel>());
            UIManager.Instance.GetUI<CharacterStatPanel>().OpenPanel();
        }
    }
}