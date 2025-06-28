using IdleProject.Battle.Character;
using IdleProject.Core;
using IdleProject.Core.UI;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using IdleProject.Data.StaticData;

namespace IdleProject.Lobby.UI.CharacterPanel
{
    public class CharacterStatPanel : UIPanel, IUISelectCharacterUpdatable, IUISelectEquipmentItemUpdatable, IUIUpdatable
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
            ShowCharacterStat(selectCharacter);
        }

        private void ShowCharacterStat(DynamicCharacterData selectCharacter)
        {
            var characterStat = selectCharacter.GetStat();
            
            _attackDamageStatBar.ShowStat(CharacterStatType.AttackDamage, characterStat.attackDamage);
            _healthPointStatBar.ShowStat(CharacterStatType.HealthPoint, characterStat.healthPoint);
            _defensePointStatBar.ShowStat(CharacterStatType.DefensePoint, characterStat.defensePoint);
            _criticalPercentStatBar.ShowStat(CharacterStatType.CriticalPercent, characterStat.criticalPercent);
            _criticalResistance.ShowStat(CharacterStatType.CriticalResistance, characterStat.criticalResistance);
        }

        public void SelectEquipmentItemUpdatable(DynamicEquipmentItemData item)
        {
            ClosePanel();
        }

        public void UpdateUI()
        {
            ShowCharacterStat(UIManager.Instance.GetUI<SelectCharacterPanel>().SelectedCharacter);
        }
    }
}
