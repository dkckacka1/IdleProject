using System.Linq;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterPanel : UIPanel, IUISelectCharacterUpdatable
    {
        public DynamicCharacterData Selector;
        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CloseCharacterPopupButton").Button.onClick.AddListener(ClosePanel);

            Selector ??= DataManager.Instance.DataController.Player.PlayerCharacterDataDic.Values.First();
        }

        public override void OpenPanel()
        {
            base.OpenPanel();

            UIManager.Instance.GetUI<SelectCharacterPanel>().SelectCharacter(Selector);
        }
        
        public void SelectCharacterUpdatable(DynamicCharacterData selectCharacter)
        {
            Selector = selectCharacter;
        }
    }
}