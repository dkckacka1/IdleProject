using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace IdleProject.Core.UI
{
    public class DropSlot : SlotUI, IDropHandler
    {
        public UnityEvent<PointerEventData> dragEvent;
        public void OnDrop(PointerEventData eventData)
        {
            dragEvent?.Invoke(eventData);
        }
    }
}