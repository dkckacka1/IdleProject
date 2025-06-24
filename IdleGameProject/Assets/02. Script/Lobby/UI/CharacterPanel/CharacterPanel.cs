using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Data;
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
            
            var mainCharacterName = DataManager.Instance.DataController.userData.UserHeroList[0].heroName;
            var mainCharacterData = DataManager.Instance.GetData<CharacterData>(mainCharacterName);

            UIManager.Instance.GetUIsOfType<ISelectCharacterUpdatableUI>()
                .ForEach(ui => ui.SetCharacter(mainCharacterData));
        }
    }
}