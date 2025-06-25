using System.Linq;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using Sirenix.Utilities;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterPanel : UIPanel
    {
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CloseCharacterPopupButton").Button.onClick.AddListener(ClosePanel);
        }

        public override void OpenPanel()
        {
            base.OpenPanel();

            var mainCharacterName = DataManager.Instance.DataController.Player.PlayerData.frontMiddleCharacterName;
            var mainCharacterData =
                DataManager.Instance.DataController.Player.PlayerCharacterDataList.FirstOrDefault(character =>
                    character.CharacterData.addressValue.characterName == mainCharacterName);

            UIManager.Instance.GetUIsOfType<IUISelectCharacterUpdatable>()
                .ForEach(ui => ui.SetCharacter(mainCharacterData));
        }
    }
}