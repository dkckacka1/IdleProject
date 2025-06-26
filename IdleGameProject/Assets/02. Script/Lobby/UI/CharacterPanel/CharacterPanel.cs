using System.Linq;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using Sirenix.Utilities;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterPanel : UIPanel, IUISelectCharacterUpdatable
    {
        public DynamicCharacterData SelectCharacter;
        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CloseCharacterPopupButton").Button.onClick.AddListener(ClosePanel);

            SelectCharacter ??= DataManager.Instance.DataController.Player.PlayerCharacterDataList[0];
        }

        public override void OpenPanel()
        {
            base.OpenPanel();
            UIManager.Instance.GetUIsOfType<IUISelectCharacterUpdatable>()
                .ForEach(ui => ui.SetCharacter(SelectCharacter));
        }

        public void SetCharacter(DynamicCharacterData character)
        {
            SelectCharacter = character;
        }
    }
}