using System;
using System.Collections.Generic;
using IdleProject.Core.Resource;
using IdleProject.Data;
using Sirenix.Utilities;
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

        private readonly Dictionary<Type, SlotParts> _slotPartsDic = new();
        
        private EventTrigger _slotEventTrigger;
        private IData _data;

        private EventTrigger SlotEventTrigger => _slotEventTrigger ??= GetComponent<EventTrigger>();

        public T GetData<T>() where T : class, IData, ISlotData => _data.GetData<T>();
        
        public void SetData<T>(T data) where T:  class, IData, ISlotData
        {
            _data = data;
            var iconName = data.GetIconName;
            slotImage.sprite = ResourceManager.Instance.GetAsset<Sprite>(iconName);
            _slotPartsDic.Values.ForEach(parts => parts.SetData(data));
        }
        
        public void SetFocus(bool isFocus)
        {
            if (focusFrameImage is null) return;
            
            focusFrameImage.enabled = isFocus;
        }
        
        public void PublishEvent<T>(EventTriggerType triggerType, UnityAction<T, SlotUI> callback) where T : BaseEventData
        {
            var entry = new EventTrigger.Entry
            {
                eventID = triggerType
            };
            
            entry.callback.AddListener(eventData=>
            { 
                callback.Invoke((T)eventData, this);
            });
            
            SlotEventTrigger.triggers.Add(entry);
        }
        
        public T GetSlotParts<T>() where T : SlotParts
        {
            if (_slotPartsDic.TryGetValue(typeof(T), out var component))
            {
                return component as T;
            }

            return null;
        }

        public void AddSlotParts<T>(T slotComponent) where T : SlotParts
        {
            var type = slotComponent.GetType();
            _slotPartsDic.TryAdd(type, slotComponent);
        }
        
        public void UnPublishAllEvent()
        {
            SlotEventTrigger.triggers.Clear();
        }
        
        private void RegisterOwnSlotComponents()
        {
            var components = GetComponents<SlotParts>();

            foreach (var component in components)
            {
                AddSlotParts(component);
            }
        }


        public static SlotUI GetSlotUI(Transform parent)
        {
            var slotObject = ResourceManager.Instance.GetPrefab(ResourceManager.SlotLabelName, "Slot").GetComponent<SlotUI>();
            var slotInstance = Instantiate(slotObject, parent);
            slotInstance.RegisterOwnSlotComponents();
            return slotInstance;
        }
        
        public static SlotUI GetSlotUI<T>(Transform parent) where T : SlotParts
        {
            var slotObject = ResourceManager.Instance.GetPrefab(ResourceManager.SlotLabelName, typeof(T).Name).GetComponent<SlotUI>();
            var slotInstance = Instantiate(slotObject, parent);
            slotInstance.RegisterOwnSlotComponents();
            return slotInstance;
        }
    }
}