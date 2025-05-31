using UnityEngine;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using IdleProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using IdleProject.Battle.Character;
using Zenject;
using Random = UnityEngine.Random;

namespace IdleProject.Battle.UI
{
    public class BattleUIController : UIController
    {
        [Inject] private BattleManager _battleManager;
        [Inject] private BattleResourceLoader _battleResourceLoader;

        [Inject(Id = "FixedCanvas")] 
        public Canvas FixedCanvas { get; private set; }

        [Inject(Id = "FluidCanvas")]
        public Canvas FluidCanvas { get; private set; }
        
        [Inject(Id = "FluidHealthBarParent")] 
        public Transform FluidHealthBarParent { get; private set; }
        
        [Inject(Id = "BattleTextParent")]
        public Transform BattleTextParent { get; private set; }

        [Inject(Id = "PlayerCharacterBannerList")]
        private List<PlayerCharacterBanner> _playerCharacterBannerList;

        private Func<BattleText> _getBattleText;

        private bool _isInitialize = false;

        public async void Initialized()
        {
            _isInitialize = false;

            await _battleResourceLoader.CreatePool(PoolableType.UI, "BattleText", BattleTextParent);
            _getBattleText = () => _battleResourceLoader.GetPoolableObject<BattleText>(PoolableType.UI, "BattleText");

            SetSpeedText();
            UIManager.Instance.GetUI<UIButton>("SpeedButton").Button.onClick.AddListener(ChangeBattleSpeed);
            UIManager.Instance.GetUI<UIButton>("PauseButton").Button.onClick.AddListener(PauseGame);
            // UIManager.Instance.GetUI<UIButton>("PausePopupExitButton").Button.onClick.AddListener(ExitBattle);
            UIManager.Instance.GetUI<UIButton>("PausePopupContinueButton").Button.onClick.AddListener(ClosePausePopup);
            UIManager.Instance.GetUI<UIButton>("PausePopupRetryButton").Button.onClick.AddListener(RetryBattle);
            
            _isInitialize = true;
        }

        public PlayerCharacterBanner GetPlayerCharacterBanner()
        {
            var targetBanner = _playerCharacterBannerList.First(banner => banner.gameObject.activeInHierarchy is false); ;
            targetBanner.gameObject.SetActive(true);
            return targetBanner;
        }

        public void ShowBattleText(string text, CharacterOffset offset)
        {
            var battleText = _getBattleText.Invoke();
            Vector3 randomPos = Random.insideUnitCircle * offset.BattleTextOffsetRadius;
            var textPosition = UIManager.GetUIInScreen(Camera.main.WorldToScreenPoint(offset.BattleTextOffset) + randomPos);
            battleText.ShowText(textPosition, text);
        }
        
        
        private void ChangeBattleSpeed()
        {
            _battleManager.NextBattleSpeed();

            SetSpeedText();
        }

        private void SetSpeedText()
        {
            UIManager.Instance.GetUI<UIText>("BattleSpeedText").Text.text = $"<size=70%>x</size>{_battleManager.currentBattleSpeed:N0}";
        }

        private void PauseGame()
        {
            _battleManager.GameStateEventBus.ChangeEvent(GameStateType.Pause);
            UIManager.Instance.GetUI<PausePopup>().OpenPopup();
        }
        
        
        private void ExitBattle()
        {
        }

        private void RetryBattle()
        {
            
        }

        private void ClosePausePopup()
        {
            _battleManager.GameStateEventBus.ChangeEvent(GameStateType.Play);
            UIManager.Instance.GetUI<PausePopup>().ClosePopup();
        }
    }
}