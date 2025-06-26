using Cysharp.Threading.Tasks;
using IdleProject.Core.UI;
using IdleProject.Data.DynamicData;
using IdleProject.Data.Player;
using UnityEngine;

namespace IdleProject.Lobby.UI.CharacterPopup
{
    public class CharacterExpChangerUI : UIBase, IUIInit
    {
        private UIText _levelText;
        private UISlider _expSlider;

        private int _getExpAmount;
        private bool _isFillPlay;
        private int _fillAmount;

        private const int FILL_AMOUNT_SPLIT = 20;
        
        public void Initialized()
        {
            _levelText = UIManager.Instance.GetUI<UIText>("CharacterLevelUpPanelLevelText");
            _expSlider = UIManager.Instance.GetUI<UISlider>("CharacterExpSlider");
        }

        public void SetPlayerCharacter(DynamicCharacterData playerCharacter)
        {
            _levelText.Text.text = playerCharacter.Level.ToString();
            _expSlider.Slider.maxValue = playerCharacter.GetLevelUpExpValue;
            _expSlider.Slider.value = playerCharacter.Exp;
        }

        
        public void AddExp(int getExp)
        {
            _getExpAmount += getExp;
            _fillAmount = Mathf.Max(1, _getExpAmount / FILL_AMOUNT_SPLIT);

            if (_isFillPlay is false)
            {
                PlayFill().Forget();
            }
        }

        private async UniTaskVoid PlayFill()
        {
            _isFillPlay = true;
            var maxExp = (int)_expSlider.Slider.maxValue;
            var currentExp = (int)_expSlider.Slider.value;
            var level = int.Parse(_levelText.Text.text);
            
            while (_getExpAmount > 0)
            {
                _fillAmount = Mathf.Min(_fillAmount, _getExpAmount);
                currentExp += _fillAmount;
                _getExpAmount -= _fillAmount;

                if (currentExp >= maxExp)
                {
                    _levelText.Text.text = (++level).ToString();
                    currentExp -= maxExp;
                    maxExp = DynamicCharacterData.GetLevelExpValue(level);
                    _expSlider.Slider.maxValue = maxExp;
                }
                
                _expSlider.Slider.value = currentExp;

                await UniTask.Delay(16);
            }

            _isFillPlay = false;
        }
    }
}