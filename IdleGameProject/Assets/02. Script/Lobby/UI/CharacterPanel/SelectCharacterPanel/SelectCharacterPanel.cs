using IdleProject.Core.UI;
using IdleProject.Data.DynamicData;

namespace IdleProject.Lobby.UI.CharacterPanel
{
    public class SelectCharacterPanel : UIPanel
    {
        public DynamicCharacterData SelectedCharacter { get; private set; }
        public DynamicEquipmentItemData SelectedEquipmentItem { get; private set; }
        
        public override void Initialized()
        {

        }

        public void SelectCharacter(DynamicCharacterData selectCharacter)
        {
            if (SelectedCharacter == selectCharacter)
                return;
            
            SelectedCharacter = selectCharacter;
            foreach (var updatable in UIManager.Instance.GetUIsOfType<IUISelectCharacterUpdatable>())
            {
                updatable.SelectCharacterUpdatable(SelectedCharacter);
            }
        }

        public void SelectEquipmentItem(DynamicEquipmentItemData selectEquipmentItem)
        {
            SelectedEquipmentItem = selectEquipmentItem;
            foreach (var updatable in UIManager.Instance.GetUIsOfType<IUISelectEquipmentItemUpdatable>())
            {
                updatable.SelectEquipmentItemUpdatable(SelectedEquipmentItem);
            }
        }
    }
}