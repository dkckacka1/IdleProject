using IdleProject.Data;
using UnityEngine;

namespace IdleProject.Core.UI.Slot
{
    [RequireComponent(typeof(SlotUI))]
    public abstract class SlotParts : MonoBehaviour
    {
        private SlotUI _slotUI;
        public SlotUI SlotUI => _slotUI ??= GetComponent<SlotUI>();

        public virtual void ShowParts<T>(T data) where T : ISlotData
        {
            
        }
    }
}