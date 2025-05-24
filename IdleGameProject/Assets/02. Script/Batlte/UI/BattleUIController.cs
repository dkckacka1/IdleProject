using UnityEngine;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;
using IdleProject.Core.ObjectPool;
using IdleProject.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IdleProject.Battle.UI
{
    public class BattleUIController : UIController
    {
        [BoxGroup("FixedGroup"), SerializeField] private Canvas fixedCanvas;

        [BoxGroup("FluidGroup"), SerializeField] private Canvas fluidCanvas;
        [BoxGroup("FluidGroup"), SerializeField] private Transform fluidHealthBarParent;
        [BoxGroup("FluidGroup"), SerializeField] private Transform battleTextParent;

        [BoxGroup("PlayerBanner"), SerializeField] private List<PlayerCharacterBanner> playerCharacterBannerList; 


        public Canvas FluidCanvas { get => fluidCanvas; }
        public Canvas FixedCanvas { get => fixedCanvas; }

        public Transform FluidHealthBarParent => fluidHealthBarParent;
        public Transform BattleTextParent => battleTextParent;

        public Func<BattleText> GetBattleText;

        private bool isInitialize = false;

        public async void initialized()
        {
            isInitialize = false;

            await ResourcesLoader.CreatePool(PoolableType.UI, "BattleText", battleTextParent);
            GetBattleText = () => ResourcesLoader.GetPoolableObject<BattleText>(PoolableType.UI, "BattleText");

            isInitialize = true;
        }

        public PlayerCharacterBanner GetPlayerCharacterBanner()
        {
            var targetBanner = playerCharacterBannerList.Where(banner => banner.gameObject.activeInHierarchy is false).First(); ;
            targetBanner.gameObject.SetActive(true);
            return targetBanner;
        }
    }
}