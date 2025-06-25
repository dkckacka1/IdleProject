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
        [SerializeField] private ScrollRect slotScrollRect;

        private readonly List<SlotUI> _slotList = new List<SlotUI>();
        private SlotUI _selectSlot;
        
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
                var userCharacterList = DataManager.Instance.DataController.Player.PlayerCharacterDataList;

                var createSlotCount = userCharacterList.Count - _slotList.Count;
                for (int i = 0; i < createSlotCount; ++i)
                    // 부족한 슬롯 생성
                {
                    _slotList.Add(CreateSlot());
                }
                
                for (int i = 0; i < _slotList.Count; ++i)
                    // 캐릭터 수만큼 슬롯에 정의
                {
                    var slot = _slotList[i];
                    if (i <= userCharacterList.Count - 1)
                    {
                        slot.SetData(userCharacterList[i]);
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

        private void ClickCharacterSlot(PointerEventData eventData, SlotUI slot)
        {
            SwapSlotFocus(slot);
            var characterData = slot.GetData<DynamicCharacterData>();

            foreach (var selectCharacterUpdatableUI in UIManager.Instance.GetUIsOfType<IUISelectCharacterUpdatable>())
            {
                selectCharacterUpdatableUI.SetCharacter(characterData);
            }
        }

        private void SwapSlotFocus(SlotUI slot)
        {
            if (_selectSlot is not null)
                _selectSlot.SetFocus(false);

            _selectSlot = slot;
            _selectSlot.SetFocus(true);
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
