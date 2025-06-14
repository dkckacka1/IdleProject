using UnityEngine;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using IdleProject.Core.ObjectPool;
using IdleProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Util.Extension;

namespace IdleProject.Battle.UI
{
    public class BattleUIController : UIController
    {
        [BoxGroup("FixedGroup"), SerializeField] private Canvas fixedCanvas;

        [BoxGroup("FluidGroup"), SerializeField] private Canvas fluidCanvas;
        [BoxGroup("FluidGroup"), SerializeField] private Transform fluidHealthBarParent;
        [BoxGroup("FluidGroup"), SerializeField] private Transform battleTextParent;

        [BoxGroup("PlayerBanner"), SerializeField] private List<PlayerCharacterBanner> playerCharacterBannerList; 


        public Canvas FluidCanvas => fluidCanvas;
        public Canvas FixedCanvas => fixedCanvas;

        public Transform FluidHealthBarParent => fluidHealthBarParent;
        public Transform BattleTextParent => battleTextParent;

        public Func<BattleText> GetBattleText;

        private bool _isInitialize = false;

        public async void Initialized()
        {
            _isInitialize = false;

            await ResourceLoader.CreatePool(PoolableType.UI, "BattleText", battleTextParent);
            GetBattleText = () => ResourceLoader.GetPoolableObject<BattleText>(PoolableType.UI, "BattleText");

            SetSpeedText();
            UIManager.Instance.GetUI<UIButton>("SpeedButton").Button.onClick.AddListener(ChangeBattleSpeed);
            UIManager.Instance.GetUI<UIButton>("PauseButton").Button.onClick.AddListener(PauseGame);
            UIManager.Instance.GetUI<UIButton>("PausePopupContinueButton").Button.onClick.AddListener(ClosePausePopup);
            UIManager.Instance.GetUI<UIButton>("PausePopupRetryButton").Button.onClick.AddListener(RetryBattle);
            UIManager.Instance.GetUI<UIButton>("PausePopupExitButton").Button.onClick.AddListener(ExitBattle);
            
            _isInitialize = true;
        }

        public PlayerCharacterBanner GetPlayerCharacterBanner()
        {
            var targetBanner = playerCharacterBannerList.First(banner => banner.gameObject.activeInHierarchy is false); ;
            targetBanner.gameObject.SetActive(true);
            return targetBanner;
        }
        
        
        private void ChangeBattleSpeed()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().NextBattleSpeed();

            SetSpeedText();
        }

        private void SetSpeedText()
        {
            UIManager.Instance.GetUI<UIText>("BattleSpeedText").Text.text = $"<size=70%>x</size>{GameManager.GetCurrentSceneManager<BattleManager>().currentBattleSpeed:N0}";
        }

        private void PauseGame()
        {
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.ChangeEvent(GameStateType.Pause);
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
            GameManager.GetCurrentSceneManager<BattleManager>().GameStateEventBus.ChangeEvent(GameStateType.Play);
            UIManager.Instance.GetUI<PausePopup>().ClosePopup();
        }
    }
}