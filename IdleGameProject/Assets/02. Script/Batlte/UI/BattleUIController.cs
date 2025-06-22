using UnityEngine;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using IdleProject.Core.ObjectPool;
using IdleProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Engine.Util.Extension;
using UnityEngine.Events;

namespace IdleProject.Battle.UI
{
    public class BattleUIController : UIController
    {
        [BoxGroup("FluidGroup"), SerializeField] private Transform fluidHealthBarParent;
        [BoxGroup("FluidGroup"), SerializeField] private Transform battleTextParent;

        [BoxGroup("BattleUI"), SerializeField] private List<PlayerCharacterBanner> playerCharacterBannerList; 
        [BoxGroup("BattleUI"), SerializeField] private ReadyPanel readyPanelUI; 

        public Transform FluidHealthBarParent => fluidHealthBarParent;

        public Func<BattleText> GetBattleText;

        public async UniTask Initialized()
        {
            await ResourceLoader.CreatePool(PoolableType.UI, "BattleText", battleTextParent);
            GetBattleText = () => ResourceLoader.GetPoolableObject<BattleText>(PoolableType.UI, "BattleText");

            SetSpeedText();
            UIManager.Instance.GetUI<UIPopup>("PausePopup").Initialized();
            UIManager.Instance.GetUI<UIPopup>("PausePopup").ClosePopup();
            UIManager.Instance.GetUI<UIPopup>("PickCharacterPopup").Initialized();
            UIManager.Instance.GetUI<UIPopup>("PickCharacterPopup").OpenPopup();
            UIManager.Instance.GetUI<UIPopup>("WinPopup").Initialized();
            UIManager.Instance.GetUI<UIPopup>("WinPopup").ClosePopup();
            UIManager.Instance.GetUI<UIButton>("SpeedButton").Button.onClick.AddListener(ChangeBattleSpeed);
            UIManager.Instance.GetUI<UIButton>("PauseButton").Button.onClick.AddListener(PauseGame);
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
    }
}