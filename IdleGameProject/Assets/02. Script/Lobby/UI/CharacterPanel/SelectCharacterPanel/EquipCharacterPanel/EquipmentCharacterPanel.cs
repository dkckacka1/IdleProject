using Cysharp.Threading.Tasks;
using IdleProject.Core.Resource;
using IdleProject.Core.UI;
using IdleProject.Core.UI.Loading;
using IdleProject.Data;
using IdleProject.Data.StaticData;
using IdleProject.Lobby.Character;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class EquipmentCharacterPanel : UIPanel, ISelectCharacterUpdatableUI
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
        
        public void SetCharacter(CharacterData selectData)
        {
            _characterLoadingRotate.StartLoading(LoadCharacter(selectData)).Forget();
        }

        private async UniTask LoadCharacter(CharacterData characterData)
        {
            var modelObject = ResourceManager.Instance.GetPrefab(ResourceManager.CharacterModelLabelName, $"Model_{characterData.addressValue.characterName}");
            var modelInstance = await InstantiateAsync(modelObject, _selectCharacter.transform).ToUniTask();
            _selectCharacter.SetModel(modelInstance[0]);

            var animatorController = ResourceManager.Instance.GetAsset<RuntimeAnimatorController>(characterData.addressValue.characterAnimationName);
            _selectCharacter.SetAnimation(animatorController);
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
        }
    }
}