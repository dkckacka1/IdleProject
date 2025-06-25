using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Core.UI.Slot
{
    public class ConsumableItemSlot : SlotComponent
    {
        [SerializeField] private Image shadowImage;
        [SerializeField] private TextMeshProUGUI haveCountText;

        public int CurrentCount { get; private set; }
        
        public void SetShadow(bool isShadow)
        {
            shadowImage.enabled = isShadow;
        }

        public void SetCount(int count, bool isShadowControl = false)
        {
            if (isShadowControl)
                SetShadow(count <= 0);

            CurrentCount = count;
            
            haveCountText.gameObject.SetActive(count > 0);
            haveCountText.text = count.ToString();
        }
    }
}
