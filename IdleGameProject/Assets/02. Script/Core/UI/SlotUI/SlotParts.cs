using System;
using IdleProject.Data;
using UnityEngine;

namespace IdleProject.Core.UI.Slot
{
    [RequireComponent(typeof(SlotUI))]
    public abstract class SlotParts : MonoBehaviour
    {
        private SlotUI _slotUI;
        public SlotUI SlotUI
        {
            get => _slotUI ??= GetComponent<SlotUI>();
            private set => _slotUI = value;
        }

        public virtual void SetData<T>(T data) where T : IData, ISlotData
        {
            
        }
    }
}