using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Loading;
using IdleProject.Core.UI.Slot;
using IdleProject.Data;
using IdleProject.Data.StaticData;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class SelectSlotPanel : UIPanel
    {
        [SerializeField] private LoadingRotateUI loadingUI;

        [SerializeField] private ScrollRect slotScrollRect;

        private readonly List<SlotUI> _slotList = new List<SlotUI>();
        
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
                var userHeroList = DataManager.Instance.DataController.userData.userHeroList;

                var createSlotCount = userHeroList.Count - _slotList.Count;
                for (int i = 0; i < createSlotCount; ++i)
                {
                    _slotList.Add(CreateSlot());
                }
                
                for (int i = 0; i < _slotList.Count; ++i)
                {
                    var slot = _slotList[i];
                    if (i <= userHeroList.Count - 1)
                    {
                        var data = DataManager.Instance.GetData<CharacterData>(userHeroList[i].heroName);

                        slot.SetData(data);
                        slot.PublishEvent<PointerEventData>(EventTriggerType.PointerClick, ClickCharacterSlot);
                        slot.gameObject.SetActive(true);
                    }
                    else
                    {
                        slot.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                foreach (var slot in _slotList)
                {
                    slot.UnPublishAllEvent();
                    slot.gameObject.SetActive(false);
                }
            }
        }

        private void ClickCharacterSlot(PointerEventData eventData)
        {
            var slot = eventData.pointerClick.GetComponent<SlotUI>();
            var characterData = slot.GetData<CharacterData>();

            foreach (var selectCharacterUpdatableUI in UIManager.Instance.GetUIsOfType<ISelectCharacterUpdatableUI>())
            {
                selectCharacterUpdatableUI.SetCharacter(characterData);
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

        private SlotUI CreateSlot()
        {
            return SlotUI.GetSlotUI(slotScrollRect.content);
        }
    }
}
