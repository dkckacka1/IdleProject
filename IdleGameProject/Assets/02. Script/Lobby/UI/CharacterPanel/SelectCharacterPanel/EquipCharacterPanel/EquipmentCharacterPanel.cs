using System;
using Cysharp.Threading.Tasks;
using Engine.Util.Extension;
using IdleProject.Core;
using IdleProject.Core.GameData;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Loading;
using IdleProject.Core.UI.Slot;
using IdleProject.Data;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using IdleProject.Lobby.Character;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class EquipmentCharacterPanel : UIPanel, IUISelectCharacterUpdatable, IUIUpdatable
    {
        private LobbyCharacter _selectCharacterModel;
        private LoadingRotateUI _characterLoadingRotate;

        private CharacterEquipSlot _helmetSlot;
        private CharacterEquipSlot _armorSlot;
        private CharacterEquipSlot _weaponSlot;
        private CharacterEquipSlot _accessorySlot;
        private CharacterEquipSlot _gloveSlot;
        private CharacterEquipSlot _bootsSLot;

        private UIText _combatPowerText;
        
        public override void Initialized()
        {
            _selectCharacterModel = UIManager.Instance.GetUI<LobbyCharacter>("CharacterPanel_Character");
            _characterLoadingRotate = UIManager.Instance.GetUI<LoadingRotateUI>("CharacterPanel_CharacterLoadingUI");

            _helmetSlot = UIManager.Instance.GetUI<CharacterEquipSlot>("EquipmentSlot_Helmet");
            _armorSlot = UIManager.Instance.GetUI<CharacterEquipSlot>("EquipmentSlot_Armor");
            _weaponSlot = UIManager.Instance.GetUI<CharacterEquipSlot>("EquipmentSlot_Weapon");
            _accessorySlot = UIManager.Instance.GetUI<CharacterEquipSlot>("EquipmentSlot_Accessory");
            _gloveSlot = UIManager.Instance.GetUI<CharacterEquipSlot>("EquipmentSlot_Glove");
            _bootsSLot = UIManager.Instance.GetUI<CharacterEquipSlot>("EquipmentSlot_Boots");

            _combatPowerText = UIManager.Instance.GetUI<UIText>("CombatPowerText");
            
            EnumExtension.Foreach<EquipmentItemType>(type => GetEquipmentSlot(type).Slot.PublishEvent<PointerEventData>(EventTriggerType.PointerClick, OnClickEquipSlot));
            
            var showCharacterStatPanelToggle = UIManager.Instance.GetUI<UIToggle>("ShowCharacterStatPanelToggle");
            showCharacterStatPanelToggle.Toggle.onValueChanged.AddListener(ShowCharacterLevelUpPanelToggle);

            var showCharacterLevelUpPanelToggle = UIManager.Instance.GetUI<UIToggle>("ShowCharacterLevelUpPanelToggle");
            showCharacterLevelUpPanelToggle.Toggle.onValueChanged.AddListener(ShowCharacterStatPanelToggle);
                
            showCharacterStatPanelToggle.Toggle.isOn = true;
            showCharacterStatPanelToggle.Toggle.onValueChanged.Invoke(true);

            showCharacterLevelUpPanelToggle.Toggle.isOn = false;
            showCharacterLevelUpPanelToggle.Toggle.onValueChanged.Invoke(false);
        }


        private async UniTask LoadCharacter(StaticCharacterData staticCharacterData)
        {
            var modelObject = ResourceManager.Instance.GetPrefab(ResourceManager.CharacterModelLabelName, $"Model_{staticCharacterData.addressValue.characterName}");
            var modelInstance = Instantiate(modelObject, _selectCharacterModel.transform);
            _selectCharacterModel.SetModel(modelInstance);
            var animatorController = ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(staticCharacterData.addressValue.characterAnimationName);
            await _selectCharacterModel.SetAnimation(animatorController);
        }
        
        private void ShowCharacterLevelUpPanelToggle(bool toggleValue)
        {
            if (toggleValue)
            {
                UIManager.Instance.GetUI<CharacterStatPanel>("CharacterStatPanel").OpenPanel();
            }
            else
            {
                UIManager.Instance.GetUI<CharacterStatPanel>("CharacterStatPanel").ClosePanel();
            }
            UIManager.Instance.GetUI<SelectEquipmentItemPanel>().ClosePanel();
        }

        private void ShowCharacterStatPanelToggle(bool toggleValue)
        {
            if (toggleValue)    
            {
                UIManager.Instance.GetUI<CharacterLevelUpPanel>("CharacterLevelUpPanel").OpenPanel();
            }
            else
            {
                UIManager.Instance.GetUI<CharacterLevelUpPanel>("CharacterLevelUpPanel").ClosePanel();
            }
            
            UIManager.Instance.GetUI<SelectEquipmentItemPanel>().ClosePanel();
        }

        public void SelectCharacterUpdatable(DynamicCharacterData selectCharacter)
        {
            _characterLoadingRotate.StartLoading(LoadCharacter(selectCharacter.StaticData)).Forget();

            ShowCharacterEquipment(selectCharacter);
        }

        private void ShowCharacterEquipment(DynamicCharacterData selectCharacter)
        {
            _combatPowerText.Text.text = selectCharacter.GetCombatPower().ToString();
            
            _helmetSlot.SetEquipmentItem(selectCharacter.GetEquipmentItem(EquipmentItemType.Helmet));
            _weaponSlot.SetEquipmentItem(selectCharacter.GetEquipmentItem(EquipmentItemType.Weapon));
            _armorSlot.SetEquipmentItem(selectCharacter.GetEquipmentItem(EquipmentItemType.Armor));
            _accessorySlot.SetEquipmentItem(selectCharacter.GetEquipmentItem(EquipmentItemType.Accessory));
            _gloveSlot.SetEquipmentItem(selectCharacter.GetEquipmentItem(EquipmentItemType.Glove));
            _bootsSLot.SetEquipmentItem(selectCharacter.GetEquipmentItem(EquipmentItemType.Boots));
        }

        private void OnClickEquipSlot(PointerEventData eventData, SlotUI slot)
        {
            var equipmentItemData = slot.GetData<DynamicEquipmentItemData>();

            if (equipmentItemData is not null)
            {
                UIManager.Instance.GetUI<SelectCharacterPanel>().SelectEquipmentItem(equipmentItemData);
            }
        }

        private CharacterEquipSlot GetEquipmentSlot(EquipmentItemType itemType)
        {
            return itemType switch
            {
                EquipmentItemType.Weapon => _weaponSlot,
                EquipmentItemType.Helmet => _helmetSlot,
                EquipmentItemType.Armor => _armorSlot,
                EquipmentItemType.Glove => _gloveSlot,
                EquipmentItemType.Boots => _bootsSLot,
                EquipmentItemType.Accessory => _accessorySlot,
                _ => throw new ArgumentOutOfRangeException(nameof(itemType), itemType, null)
            };
        }

        public void UpdateUI()
        {
            ShowCharacterEquipment(UIManager.Instance.GetUI<SelectCharacterPanel>().SelectedCharacter);
        }
    }
}