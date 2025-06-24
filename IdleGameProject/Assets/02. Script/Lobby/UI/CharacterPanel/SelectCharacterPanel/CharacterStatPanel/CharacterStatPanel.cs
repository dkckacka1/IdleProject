using IdleProject.Battle.Character;
using IdleProject.Core.UI;
using IdleProject.Data;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterStatPanel : UIPanel, ISelectCharacterUpdatableUI
    {
        private CharacterStatBar _attackDamageStatBar;
        private CharacterStatBar _healthPointStatBar;
        private CharacterStatBar _movementSpeedStatBar;
        
        public override void Initialized()
        {
            _attackDamageStatBar = UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_AttackDamage");
            _healthPointStatBar = UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_Health");
            _movementSpeedStatBar = UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_MovementSpeed");
        }

        public void SetCharacter(CharacterData characterData)
        {
            _attackDamageStatBar.ShowStat(CharacterStatType.AttackDamage, characterData.stat.attackDamage);
            _healthPointStatBar.ShowStat(CharacterStatType.HealthPoint, characterData.stat.healthPoint);
            _movementSpeedStatBar.ShowStat(CharacterStatType.MovementSpeed, characterData.stat.movementSpeed);
        }
    }
}
