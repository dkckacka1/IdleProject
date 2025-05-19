using IdleProject.Battle.Character;
using IdleProject.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Battle.UI
{
    public class FluidHealthBar : UIBase
    {
        [SerializeField] private Slider healthFillSlider;
        [SerializeField] private Slider damageFillSlider;

        public void Initialized(float maxHealthPoint)
        {
            healthFillSlider.maxValue = maxHealthPoint;
            damageFillSlider.maxValue = maxHealthPoint;

            healthFillSlider.value = maxHealthPoint;
            damageFillSlider.value = maxHealthPoint;
        }

        public void ChangeCurrentHealthPoint(float currentHealthPoint)
        {
            healthFillSlider.value = currentHealthPoint;
            damageFillSlider.value = currentHealthPoint;
        }
    }
}