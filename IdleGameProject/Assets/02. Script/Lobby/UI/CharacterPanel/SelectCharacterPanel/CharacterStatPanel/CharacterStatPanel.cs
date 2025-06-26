using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.UI;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterStatPanel : UIPanel, IUISelectCharacterUpdatable, IUISelectEquipmentItemUpdatable
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
        
        public void SelectCharacter(DynamicCharacterData selectData)
        {
            _attackDamageStatBar.ShowStat(CharacterStatType.AttackDamage, selectData.Stat.attackDamage);
            _healthPointStatBar.ShowStat(CharacterStatType.HealthPoint, selectData.Stat.healthPoint);
            _movementSpeedStatBar.ShowStat(CharacterStatType.MovementSpeed, selectData.Stat.movementSpeed);
        }

        public void SelectEquipmentItem(StaticEquipmentItemData item)
        {
            ClosePanel();
        }
    }
}
