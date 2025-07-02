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
        private UIToggle _skillToggle;
        
        public override void Initialized()
        {
            _characterSlotToggle = UIManager.Instance.GetUI<UIToggle>("CharacterSelectToggle");
            _equipmentItemToggle = UIManager.Instance.GetUI<UIToggle>("SkillSelectToggle");
            _skillToggle = UIManager.Instance.GetUI<UIToggle>("EquipmentToggle");
            
            _characterSlotToggle.Toggle.onValueChanged.AddListener(CharacterSelectToggleValueChanged);
            _equipmentItemToggle.Toggle.onValueChanged.AddListener(SkillSelectToggleValueChanged);
            _skillToggle.Toggle.onValueChanged.AddListener(EquipmentToggleValueChanged);

            OpenPanel(SlotPanelType.Character);
        }

        public void OpenPanel(SlotPanelType panelType)
        {
            switch(panelType)
            {
                case SlotPanelType.Character:
                    _characterSlotToggle.Toggle.isOn = true;
                    _characterSlotToggle.Toggle.onValueChanged.Invoke(true);

                    _equipmentItemToggle.Toggle.isOn = false;
                    _equipmentItemToggle.Toggle.onValueChanged.Invoke(false);

                    _skillToggle.Toggle.isOn = false;
                    _skillToggle.Toggle.onValueChanged.Invoke(false);
                    break;
                case SlotPanelType.EquipmentItem:
                    _characterSlotToggle.Toggle.isOn = false;
                    _characterSlotToggle.Toggle.onValueChanged.Invoke(false);

                    _equipmentItemToggle.Toggle.isOn = true;
                    _equipmentItemToggle.Toggle.onValueChanged.Invoke(true);

                    _skillToggle.Toggle.isOn = false;
                    _skillToggle.Toggle.onValueChanged.Invoke(false);
                    break;
                case SlotPanelType.Skill:
                    _characterSlotToggle.Toggle.isOn = false;
                    _characterSlotToggle.Toggle.onValueChanged.Invoke(false);

                    _equipmentItemToggle.Toggle.isOn = false;
                    _equipmentItemToggle.Toggle.onValueChanged.Invoke(false);

                    _skillToggle.Toggle.isOn = true;
                    _skillToggle.Toggle.onValueChanged.Invoke(true);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(panelType), panelType, null);
            }
        }
        

        private void CharacterSelectToggleValueChanged(bool value)
        {
            if (value)
            {
                UIManager.Instance.GetUI<UIPanel>("CharacterSlotPanel").OpenPanel();
            }
            else
            {
                UIManager.Instance.GetUI<UIPanel>("CharacterSlotPanel").ClosePanel();
            }
        }

        private void EquipmentToggleValueChanged(bool value)
        {
            if (value)
            {
                UIManager.Instance.GetUI<UIPanel>("EquipmentItemSlotPanel").OpenPanel();
            }
            else
            {
                UIManager.Instance.GetUI<UIPanel>("EquipmentItemSlotPanel").ClosePanel();
            }
        }

        private void SkillSelectToggleValueChanged(bool value)
        {
        }
    }
}
