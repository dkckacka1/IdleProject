using System.Collections.Generic;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Loading;
using IdleProject.Core.UI.Slot;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class SelectSlotPanel : UIPanel
    {
        [SerializeField] private LoadingRotateUI loadingUI;
        
        public override void Initialized()
        {
            UIManager.Instance.GetUI<UIToggle>("CharacterSelectToggle").Toggle.onValueChanged.AddListener(CharacterSelectToggleValueChanged);
            UIManager.Instance.GetUI<UIToggle>("SkillSelectToggle").Toggle.onValueChanged.AddListener(SkillSelectToggleValueChanged);
            UIManager.Instance.GetUI<UIToggle>("EquipmentToggle").Toggle.onValueChanged.AddListener(EquipmentToggleValueChanged);
            
            UIManager.Instance.GetUI<UIToggle>("CharacterSelectToggle").Toggle.isOn = true;
            UIManager.Instance.GetUI<UIToggle>("CharacterSelectToggle").Toggle.onValueChanged.Invoke(true);
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
            Debug.Log($"SkillSelectToggle {value}"); 
        }

        private void SkillSelectToggleValueChanged(bool value)
        {
            Debug.Log($"EquipmentToggle {value}"); 
        }
    }
}
