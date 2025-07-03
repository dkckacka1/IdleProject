using TMPro;
using UnityEngine;

namespace IdleProject.Core.UI
{
    [RequireComponent(typeof(TMP_Dropdown))]
    public class UIDropdown : UIBase
    {
        public TMP_Dropdown Dropdown { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Dropdown = GetComponent<TMP_Dropdown>();
        }
    }
}