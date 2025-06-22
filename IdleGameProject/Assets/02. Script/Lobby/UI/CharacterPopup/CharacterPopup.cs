using IdleProject.Core.UI;
using IdleProject.Lobby.Character;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterPopup : UIPopup
    {
        [SerializeField] private LobbyCharacter lobbyCharacter;

        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CloseCharacterPopupButton").Button.onClick.AddListener(ClosePopup);
            UIManager.Instance.GetUI<UIPanel>("SelectCharacterPanel").Initialized();
            UIManager.Instance.GetUI<UIPanel>("SelectSlotPanel").Initialized(); 
        }
    }
}