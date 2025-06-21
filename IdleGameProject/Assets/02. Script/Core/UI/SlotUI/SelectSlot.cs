using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace IdleProject.Core.UI
{
    public class SelectSlot : SlotUI, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [HideInInspector] public UnityEvent<PointerEventData> dragEvent;
        [HideInInspector] public UnityEvent<PointerEventData> beginDragEvent;
        [HideInInspector] public UnityEvent<PointerEventData> endDragEvent;
                
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