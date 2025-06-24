using IdleProject.Core.GameData;
using IdleProject.Core.Resource;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace  IdleProject.Core.UI.Slot
{
    [RequireComponent(typeof(EventTrigger))]
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] private Image slotFrameImage;
        [SerializeField] private Image slotBackground;
        [SerializeField] private Image slotImage;
        [SerializeField] private Image focusFrameImage;

        private EventTrigger _slotEventTrigger;
        
        private Data.StaticData.Data _data;

        private EventTrigger SlotEventTrigger
        {
            get => _slotEventTrigger ??= GetComponent<EventTrigger>();
            set => _slotEventTrigger = value;
        }

        public T GetData<T>() where T : Data.StaticData.Data => _data as T;

        public void SetData<T>(T data) where T:  Data.StaticData.Data, ISlotData
        {
            _data = data;
            var iconName = data.GetIconName;
            slotImage.sprite = ResourceManager.Instance.GetAsset<Sprite>(iconName);
        }
        
        public void SetFocus(bool isFocus)
        {
            if (focusFrameImage is null) return;
            
            focusFrameImage.enabled = isFocus;
        }

        public void PublishEvent<T>(EventTriggerType triggerType, UnityAction<T> callback) where T : BaseEventData
        {
            var entry = new EventTrigger.Entry
            {
                eventID = triggerType
            };
            
            entry.callback.AddListener(eventData =>
            {
                callback.Invoke((T)eventData);
            });
            
            SlotEventTrigger.triggers.Add(entry);
        }
        
        public void UnPublishAllEvent()
        {
            SlotEventTrigger.triggers.Clear();
        }

        public static SlotUI GetSlotUI(Transform parent)
        {
            var slotObject = ResourceManager.Instance.GetPrefab(ResourceManager.SlotLabelName, "Slot").GetComponent<SlotUI>();
            return Instantiate(slotObject, parent);
        }
        
        public static SlotUI GetSlotUI<T>(Transform parent) where T : SlotComponent
        {
            var slotObject = ResourceManager.Instance.GetPrefab(ResourceManager.SlotLabelName, typeof(T).Name).GetComponent<SlotUI>();
            return Instantiate(slotObject, parent);
        }
    }
}