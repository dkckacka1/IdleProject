using Cysharp.Threading.Tasks;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Loading;
using IdleProject.Data;
using IdleProject.Data.DynamicData;
using IdleProject.Data.StaticData;
using IdleProject.Lobby.Character;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class EquipmentCharacterPanel : UIPanel, IUISelectCharacterUpdatable
    {
        private LobbyCharacter _selectCharacter;
        private LoadingRotateUI _characterLoadingRotate;
        
        public override void Initialized()
        {
            _selectCharacter = UIManager.Instance.GetUI<LobbyCharacter>("CharacterPanel_Character");
            _characterLoadingRotate = UIManager.Instance.GetUI<LoadingRotateUI>("CharacterPanel_CharacterLoadingUI");
            
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
            var modelInstance = Instantiate(modelObject, _selectCharacter.transform);
            _selectCharacter.SetModel(modelInstance);
            var animatorController = ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(staticCharacterData.addressValue.characterAnimationName);
            await _selectCharacter.SetAnimation(animatorController);
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

        public void SelectCharacter(DynamicCharacterData selectData)
        {
            _characterLoadingRotate.StartLoading(LoadCharacter(selectData.CharacterData)).Forget();
        }
    }
}