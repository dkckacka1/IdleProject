using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace IdleProject.Core.UI
{
    public class SelectSlot : SlotUI, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public UnityEvent<PointerEventData> dragEvent;
        public UnityEvent<PointerEventData> beginDragEvent;
        public UnityEvent<PointerEventData> endDragEvent;
                
        public void OnDrag(PointerEventData eventData)
        {
            dragEvent?.Invoke(eventData);
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            beginDragEvent?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            endDragEvent?.Invoke(eventData);
        }
    }
}