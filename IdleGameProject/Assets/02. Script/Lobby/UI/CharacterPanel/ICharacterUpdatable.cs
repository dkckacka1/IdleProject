using IdleProject.Data.StaticData;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    // 캐릭터 선택창에서 캐릭터를 선택함으로써 변경되는 UI
    public interface ISelectCharacterUpdatableUI
    {
        public void SetCharacter(CharacterData selectData);
    }
}