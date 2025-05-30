using Engine.Core.Time;
using IdleProject.Core.UI;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Battle.UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider healthFillSlider;
        [SerializeField] private Slider damageFillSlider;

        [SerializeField] private float damagePlayDuration = 1f;
        private float _damageAmount = 0f;

        public void Initialized(float maxHealthPoint)
        {
            healthFillSlider.maxValue = maxHealthPoint;
            damageFillSlider.maxValue = maxHealthPoint;

            healthFillSlider.value = maxHealthPoint;
            damageFillSlider.value = maxHealthPoint;

            _damageAmount = 0f;
        }

        public void OnChangeHealthPoint(float currentHealthPoint)
        {
            healthFillSlider.value = currentHealthPoint;

            if (healthFillSlider.value > damageFillSlider.value)
            {
                damageFillSlider.value = healthFillSlider.value;
            }

            _damageAmount = damageFillSlider.value - healthFillSlider.value;
        }

        public void PlayDamageSlider()
        {
            if (_damageAmount > 0)
            {
                damageFillSlider.value -= _damageAmount / damagePlayDuration * BattleManager.GetCurrentBattleSpeed * Time.deltaTime;
            }

            if(healthFillSlider.value >= damageFillSlider.value)
            {
                _damageAmount = 0;
            }
        }
    }
}