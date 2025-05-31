using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace IdleProject.Battle.UI
{
    public class BattleUIInstaller : MonoInstaller
    {
        [BoxGroup("FixedGroup"), SerializeField]
        private Canvas fixedCanvas;

        [BoxGroup("FixedGroup"), SerializeField]
        private Transform playerCharacterBannerParent;

        [BoxGroup("FluidGroup"), SerializeField]
        private Canvas fluidCanvas;

        [BoxGroup("FluidGroup"), SerializeField]
        private Transform fluidHealthBarParent;

        [BoxGroup("FluidGroup"), SerializeField]
        private Transform battleTextParent;

        public override void InstallBindings()
        {
            Container.Bind<BattleUIController>().FromComponentInHierarchy().AsSingle();
            
            Container.Bind<Canvas>().WithId("FixedCanvas").FromInstance(fixedCanvas).AsCached();
            Container.Bind<Canvas>().WithId("FluidCanvas").FromInstance(fluidCanvas).AsCached();

            Container.Bind<Transform>().WithId("FluidHealthBarParent").FromInstance(fluidHealthBarParent).AsCached();
            Container.Bind<Transform>().WithId("BattleTextParent").FromInstance(battleTextParent).AsCached();

            var bannerList = playerCharacterBannerParent.GetComponentsInChildren<PlayerCharacterBanner>(true).ToList();
            Container.Bind<List<PlayerCharacterBanner>>().WithId("PlayerCharacterBannerList").FromInstance(bannerList).AsCached();
        }
    }
}