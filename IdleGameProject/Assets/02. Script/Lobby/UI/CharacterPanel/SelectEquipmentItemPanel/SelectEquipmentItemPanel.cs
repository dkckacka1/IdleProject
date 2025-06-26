using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class SelectEquipmentItemPanel : UIPanel, IUISelectEquipmentItemUpdatable
    {
        [SerializeField] private EquipmentItemSlot itemSlot;

        private UIText _equipmentItemNameText;
        private CharacterStatBar _equipmentItemFirstStatBar;
        private CharacterStatBar _equipmentItemSecondStatBar;
        
        private DynamicEquipmentItemData _selectEquipmentItem;
        
        
        public override void Initialized()
        {
            _equipmentItemNameText = UIManager.Instance.GetUI<UIText>("EquipmentItemText");
            _equipmentItemFirstStatBar = UIManager.Instance.GetUI<CharacterStatBar>("EquipmentFirstValueBar");
            _equipmentItemSecondStatBar = UIManager.Instance.GetUI<CharacterStatBar>("EquipmentSecondValueBar");
        }

        private void ShowEquipmentItemDetail(DynamicEquipmentItemData item)
        {
            itemSlot.SlotUI.SetData(item);
            itemSlot.SetData(item);
            var currentEquippedCharacter = string.IsNullOrEmpty(item.equipmentCharacterName)
                ? null
                : DataManager.Instance.GetData<StaticCharacterData>(item.equipmentCharacterName);
            itemSlot.SetEquipmentCharacterIcon(currentEquippedCharacter);
            
            _equipmentItemNameText.Text.text = item.StaticData.itemName;
        }
        
        public void SelectEquipmentItem(DynamicEquipmentItemData item)
        {
            OpenPanel();

            _selectEquipmentItem = item;
            ShowEquipmentItemDetail(item);
        }
    }
}
