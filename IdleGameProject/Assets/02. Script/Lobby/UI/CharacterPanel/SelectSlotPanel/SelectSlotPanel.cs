using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using IdleProject.Core.GameData;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Loading;
using IdleProject.Data;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class SelectSlotPanel : UIPanel
    {
        [SerializeField] private SelectSlot selectSlotPrefab;
        [SerializeField] private LoadingRotateUI loadingUI;

        [SerializeField] private Transform content;

        private readonly List<SlotUI> _slotList = new List<SlotUI>();
        
        public override async UniTask Initialized()
        {
            UIManager.Instance.GetUI<UIToggle>("CharacterSelectToggle").Toggle.onValueChanged.AddListener(CharacterSelectToggleValueChanged);
            UIManager.Instance.GetUI<UIToggle>("SkillSelectToggle").Toggle.onValueChanged.AddListener(SkillSelectToggleValueChanged);
            UIManager.Instance.GetUI<UIToggle>("EquipmentToggle").Toggle.onValueChanged.AddListener(EquipmentToggleValueChanged);
            
            // 초기 오픈시 세팅
            UIManager.Instance.GetUI<UIToggle>("CharacterSelectToggle").Toggle.isOn = true;
            CharacterSelectToggleValueChanged(true);
        }

        private void CharacterSelectToggleValueChanged(bool value)
        {
            if (value)
            {
                var userHeroList = DataManager.Instance.DataController.userData.UserHeroList;

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
                        slot.clickEvent.AddListener(ClickCharacterSlot);
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
                    slot.RemoveAllEvent();
                    slot.gameObject.SetActive(false);
                }
            }
        }

        private void ClickCharacterSlot(PointerEventData eventData)
        {
            var slot = eventData.pointerClick.GetComponent<SlotUI>();
            var characterData = slot.GetData<CharacterData>();

            var equipmentCharacterPanel = UIManager.Instance.GetUI<SelectCharacterPanel>("SelectCharacterPanel");
            equipmentCharacterPanel.SetCharacter(characterData.addressValue.characterName);
        }

        private void EquipmentToggleValueChanged(bool value)
        {
            Debug.Log($"SkillSelectToggle {value}"); 
        }

        private void SkillSelectToggleValueChanged(bool value)
        {
            Debug.Log($"EquipmentToggle {value}"); 
        }

        private SelectSlot CreateSlot()
        {
            return Instantiate(selectSlotPrefab, content);
        }
    }
}
