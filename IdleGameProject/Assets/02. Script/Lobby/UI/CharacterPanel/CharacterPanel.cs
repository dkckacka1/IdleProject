using System.Linq;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;

namespace IdleProject.Lobby.UI.CharacterPanel
{
    public class CharacterPanel : UIPanelCanvas
    {
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CloseCharacterPanelButton").Button.onClick.AddListener(ClosePanel);
        }

        public override void OpenPanel()
        {
            base.OpenPanel();

            var selectedCharacter = UIManager.Instance.GetUI<SelectCharacterPanel>().SelectedCharacter;
            if (selectedCharacter is null)
            {
                UIManager.Instance.GetUI<SelectCharacterPanel>().SelectCharacter(DataManager.Instance.DataController.Player.PlayerCharacterDataDic.Values.First());
            }
        }
    }
}