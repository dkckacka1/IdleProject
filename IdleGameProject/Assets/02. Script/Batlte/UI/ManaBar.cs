using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Battle.UI
{
    public class ManaBar : MonoBehaviour
    {
        [SerializeField] private Slider manaBar;

        public void Initialized(float manaPoint)
        {
            manaBar.maxValue = manaPoint;
        }

        public void OnChangeManaPoint(float currentManaPoint)
        {
            manaBar.value = currentManaPoint;
        }
    }
}