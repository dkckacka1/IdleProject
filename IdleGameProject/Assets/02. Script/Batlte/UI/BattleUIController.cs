using UnityEngine;
using IdleProject.Core.UI;
using Sirenix.OdinInspector;

namespace IdleProject.Battle.UI
{
    public class BattleUIController : UIController
    {
        [BoxGroup("FluidGroup"), SerializeField] private Canvas fluidCanvas;

        [BoxGroup("FixedGroup"), SerializeField] private Canvas FixedCanvas;


        public Canvas FluidCanvas { get => fluidCanvas; }
        public Canvas FixedCanvas1 { get => FixedCanvas; }
    }
}