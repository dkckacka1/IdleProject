using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    // 캐릭터 선택창에서 캐릭터를 선택함으로써 변경되는 UI
    public interface IUISelectCharacterUpdatable
    {
        public void SelectCharacterUpdatable(DynamicCharacterData selectCharacter);
    }
}