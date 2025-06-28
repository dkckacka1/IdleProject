using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using Sirenix.Utilities;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPanel
{
    public class SelectEquipmentItemPanel : UIPanel, IUISelectEquipmentItemUpdatable, IUIUpdatable
    {
        [SerializeField] private EquipmentItemSlot itemSlot;

        private UIText _equipmentItemNameText;
        private CharacterStatBar _equipmentItemFirstStatBar;
        private CharacterStatBar _equipmentItemSecondStatBar;

        private DynamicCharacterData GetSelectedCharacter =>
            UIManager.Instance.GetUI<SelectCharacterPanel>().SelectedCharacter;

        private DynamicEquipmentItemData GetSelectedEquipmentItem =>
            UIManager.Instance.GetUI<SelectCharacterPanel>().SelectedEquipmentItem;
        
        
        public override void Initialized()
        {
            _equipmentItemNameText = UIManager.Instance.GetUI<UIText>("EquipmentItemText");
            _equipmentItemFirstStatBar = UIManager.Instance.GetUI<CharacterStatBar>("EquipmentFirstValueBar");
            _equipmentItemSecondStatBar = UIManager.Instance.GetUI<CharacterStatBar>("EquipmentSecondValueBar");
            
            UIManager.Instance.GetUI<UIButton>("EquipItemButton").Button.onClick.AddListener(EquipItem);
            UIManager.Instance.GetUI<UIButton>("ReleaseItemButton").Button.onClick.AddListener(ReleaseItem);
        }

        private void ShowEquipmentItemDetail(DynamicEquipmentItemData item)
        {
            itemSlot.SlotUI.BindData(item);
            itemSlot.ShowParts(item);
            var currentEquippedCharacter = string.IsNullOrEmpty(item.equipmentCharacterName)
                ? null
                : DataManager.Instance.GetData<StaticCharacterData>(item.equipmentCharacterName);
            itemSlot.SetEquipmentCharacterIcon(currentEquippedCharacter);
            
            _equipmentItemNameText.Text.text = item.StaticData.itemName;

            _equipmentItemFirstStatBar.gameObject.SetActive(item.StaticData.itemFirstValue > 0);
            _equipmentItemFirstStatBar.ShowStat(item.StaticData.firstValueStatType, item.StaticData.itemFirstValue);
            _equipmentItemSecondStatBar.gameObject.SetActive(item.StaticData.itemSecondValue > 0);
            _equipmentItemSecondStatBar.ShowStat(item.StaticData.secondValueStatType, item.StaticData.itemSecondValue);
        }
        
        private void EquipItem()
        {
            var selectedEquipmentItem = GetSelectedEquipmentItem;
            var selectedCharacter = GetSelectedCharacter;
            
            // 기존 장착한 무기
            var prevEquipmentItem = selectedCharacter.GetEquipmentItem(selectedEquipmentItem.StaticData.itemType);
            
            // 기존 장착한 캐릭터가 있었다면
            var prevEquippedCharacter = selectedEquipmentItem.GetEquippedCharacter();
            prevEquippedCharacter?.ReleaseEquipmentItem(selectedEquipmentItem.StaticData.itemType);

            // 선택한 캐릭터 장비 장착
            selectedCharacter.SetEquipmentItem(selectedEquipmentItem.StaticData.itemType, selectedEquipmentItem);
            
            UIManager.Instance.GetUIsOfType<IUIUpdatable>()
                .ForEach(updatable => updatable.UpdateUI());
            
            DataManager.Instance.SaveController.Save(selectedCharacter, selectedEquipmentItem, prevEquippedCharacter, prevEquipmentItem);
        }
        
        private void ReleaseItem()
        {
            var selectedEquipmentItem = GetSelectedEquipmentItem;
            
            // 장착중인 캐릭터에서 장착 해제
            var prevEquippedCharacter = selectedEquipmentItem.GetEquippedCharacter();
            prevEquippedCharacter?.ReleaseEquipmentItem(selectedEquipmentItem.StaticData.itemType);
            
            UIManager.Instance.GetUIsOfType<IUIUpdatable>()
                .ForEach(updatable => updatable.UpdateUI());
            
            DataManager.Instance.SaveController.Save(selectedEquipmentItem, prevEquippedCharacter);
        }
        
        public void SelectEquipmentItemUpdatable(DynamicEquipmentItemData item)
        {
            OpenPanel();
            
            ShowEquipmentItemDetail(item);
        }
        
        public void UpdateUI()
        {
            var selectedItem = UIManager.Instance.GetUI<SelectCharacterPanel>().SelectedEquipmentItem;
            if (selectedItem is not null)
            {
                ShowEquipmentItemDetail(UIManager.Instance.GetUI<SelectCharacterPanel>().SelectedEquipmentItem);
            }
        }
    }
}
