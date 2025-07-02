using System;
using IdleProject.Core;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Loading;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPanel
{
    public class SelectSlotPanel : UIPanel
    {
        [SerializeField] private LoadingRotateUI loadingUI;

        private UIToggle _characterSlotToggle;
        private UIToggle _equipmentItemToggle;

        private readonly PanelRadioGroup _panelRadioGroup = new();
        
        public override void Initialized()
        {
            _characterSlotToggle = UIManager.Instance.GetUI<UIToggle>("CharacterSelectToggle");
            _equipmentItemToggle = UIManager.Instance.GetUI<UIToggle>("EquipmentToggle");
            
            _characterSlotToggle.Toggle.onValueChanged.AddListener(CharacterSelectToggleValueChanged);
            _equipmentItemToggle.Toggle.onValueChanged.AddListener(EquipmentToggleValueChanged);
            
            _panelRadioGroup.PublishPanel(UIManager.Instance.GetUI<CharacterSlotPanel>());
            _panelRadioGroup.PublishPanel(UIManager.Instance.GetUI<EquipmentItemSlotPanel>());
            
            OpenPanel(SlotPanelType.Character);
        }

        public void OpenPanel(SlotPanelType panelType)
        {
            switch(panelType)
            {
                case SlotPanelType.Character:
                    _characterSlotToggle.Toggle.isOn = true;
                    _characterSlotToggle.Toggle.onValueChanged.Invoke(true);
                    break;
                case SlotPanelType.EquipmentItem:
                    _equipmentItemToggle.Toggle.isOn = true;
                    _equipmentItemToggle.Toggle.onValueChanged.Invoke(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(panelType), panelType, null);
            }
        }
        

        private void CharacterSelectToggleValueChanged(bool value)
        {
            if (value)
            {
                UIManager.Instance.GetUI<CharacterSlotPanel>().OpenPanel();
            }
        }

        private void EquipmentToggleValueChanged(bool value)
        {
            if (value)
            {
                UIManager.Instance.GetUI<EquipmentItemSlotPanel>().OpenPanel();
            }
        }
    }
}
