using System;
using UnityEngine;

namespace IdleProject.Core.UI.Slot
{
    [RequireComponent(typeof(SlotUI))]
    public abstract class SlotComponent : MonoBehaviour
    {
        public SlotUI SlotUI { get; private set; }

        private void Awake()
        {
            SlotUI = GetComponent<SlotUI>();
        }
    }
}