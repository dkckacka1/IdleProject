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
        private CharacterStatBar _defensePointStatBar;
        private CharacterStatBar _criticalPercentStatBar;
        private CharacterStatBar _criticalResistance;
        
        public override void Initialized()
        {
            _attackDamageStatBar = UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_AttackDamage");
            _healthPointStatBar = UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_Health");
            _defensePointStatBar = UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_DefensePoint");
            _criticalPercentStatBar = UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_CriticalPercent");
            _criticalResistance = UIManager.Instance.GetUI<CharacterStatBar>("CharacterStatBar_CriticalResistance");
        }
        
        public void SelectCharacterUpdatable(DynamicCharacterData selectCharacter)
        {
            _attackDamageStatBar.ShowStat(CharacterStatType.AttackDamage, selectCharacter.Stat.attackDamage);
            _healthPointStatBar.ShowStat(CharacterStatType.HealthPoint, selectCharacter.Stat.healthPoint);
            _defensePointStatBar.ShowStat(CharacterStatType.DefensePoint, selectCharacter.Stat.defensePoint);
            _criticalPercentStatBar.ShowStat(CharacterStatType.CriticalPercent, selectCharacter.Stat.criticalPercent);
            _criticalResistance.ShowStat(CharacterStatType.CriticalResistance, selectCharacter.Stat.criticalResistance);
        }
        
        public void SelectEquipmentItemUpdatable(DynamicEquipmentItemData item)
        {
            ClosePanel();
        }
    }
}
