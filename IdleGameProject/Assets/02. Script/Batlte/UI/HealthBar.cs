using IdleProject.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Battle.UI
{
    public class HealthBar : UIBase
    {
        [SerializeField] private Slider healthFillSlider;
        [SerializeField] private Slider damageFillSlider;

        [SerializeField] private float damagePlayDuration = 1f;
        private float damageAmount = 0f;

        public void Initialized(float maxHealthPoint)
        {
            healthFillSlider.maxValue = maxHealthPoint;
            damageFillSlider.maxValue = maxHealthPoint;

            healthFillSlider.value = maxHealthPoint;
            damageFillSlider.value = maxHealthPoint;

            damageAmount = 0f;
        }

        public void OnChangeHealthPoint(float currentHealthPoint)
        {
            healthFillSlider.value = currentHealthPoint;

            if (healthFillSlider.value > damageFillSlider.value)
            {
                damageFillSlider.value = healthFillSlider.value;
            }

            damageAmount = damageFillSlider.value - healthFillSlider.value;
        }

        public void PlayDamageSlider()
        {
            if (damageAmount > 0)
            {
                damageFillSlider.value -= damageAmount * Time.deltaTime / damagePlayDuration;
            }

            if(healthFillSlider.value >= damageFillSlider.value)
            {
                damageAmount = 0;
            }
        }
    }
}