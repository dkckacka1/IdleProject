using System;
using UnityEngine;

namespace IdleProject.Core.UI.Slot
{
    [RequireComponent(typeof(SlotUI))]
    public abstract class SlotComponent : MonoBehaviour
    {
        private SlotUI _slotUI;
        public SlotUI SlotUI
        {
            get => _slotUI ??= GetComponent<SlotUI>();
            private set => _slotUI = value;
        }
    }
}