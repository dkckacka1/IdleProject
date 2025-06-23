using IdleProject.Core.UI;
using IdleProject.Lobby.Character;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterPanel : UIPanel
    {
        [SerializeField] private LobbyCharacter lobbyCharacter;

        protected override void Initialized()
        {
            UIManager.Instance.GetUI<UIButton>("CloseCharacterPopupButton").Button.onClick.AddListener(ClosePanel);
        }
    }
}