using UnityEngine;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using IdleProject.Core.ObjectPool;
using IdleProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Engine.Core.EventBus;
using Engine.Util.Extension;
using UnityEngine.Events;

namespace IdleProject.Battle.UI
{
    public class BattleUIController : UIController, IEnumEvent<BattleStateType>
    {
        [BoxGroup("BattleCanvas"), SerializeField] private Canvas battleCanvas; 
        [BoxGroup("BattleCanvas"), SerializeField] private List<PlayerCharacterBanner> playerCharacterBannerList;

        [BoxGroup("BattleFluidCanvas"), SerializeField] private Canvas battleFluidCanvas;
        [BoxGroup("BattleFluidCanvas"), SerializeField] private Transform fluidHealthBarParent;
        [BoxGroup("BattleFluidCanvas"), SerializeField] private Transform battleTextParent;

        [BoxGroup("BattlePopupCanvas"), SerializeField]
        private Canvas battlePopupCanvas;

        public Transform FluidHealthBarParent => fluidHealthBarParent;

        public Func<BattleText> GetBattleText;

        public async UniTask Initialized()
        {
            await ResourceLoader.CreatePool(PoolableType.UI, "BattleText", battleTextParent);
            GetBattleText = () => ResourceLoader.GetPoolableObject<BattleText>(PoolableType.UI, "BattleText");

            SetSpeedText();
            UIManager.Instance.GetUI<UIPanel>("PausePanel").ClosePanel();
            UIManager.Instance.GetUI<UIPanel>("PickCharacterPanel").OpenPanel();
            UIManager.Instance.GetUI<UIPanel>("WinPanel").ClosePanel();
            UIManager.Instance.GetUI<UIPanel>("DefeatPanel").ClosePanel();
            UIManager.Instance.GetUI<UIButton>("SpeedButton").Button.onClick.AddListener(ChangeBattleSpeed);
            UIManager.Instance.GetUI<UIButton>("PauseButton").Button.onClick.AddListener(PauseGame);
            
            GameManager.GetCurrentSceneManager<BattleManager>().BattleStateEventBus.PublishEvent(this);
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
            UIManager.Instance.GetUI<PausePanel>().OpenPanel();
        }

        public void OnEnumChange(BattleStateType type)
        {
            switch (type)
            {
                case BattleStateType.Ready:
                case BattleStateType.Battle:
                case BattleStateType.Skill:
                    break;
                case BattleStateType.Win:
                    battleCanvas.enabled = false;
                    battleFluidCanvas.enabled = false;
                    
                    UIManager.Instance.GetUI<UIPanel>("WinPanel").OpenPanel();
                    break;
                case BattleStateType.Defeat:
                    battleCanvas.enabled = false;
                    battleFluidCanvas.enabled = false;
                    
                    UIManager.Instance.GetUI<UIPanel>("DefeatPanel").OpenPanel();
                    break;
            }
        }
    }
}