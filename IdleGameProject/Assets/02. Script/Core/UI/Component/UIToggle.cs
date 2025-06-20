using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Core.UI
{
    [RequireComponent(typeof(Toggle))]
    public class UIToggle : UIBase
    {
        public Toggle Toggle { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Toggle = GetComponent<Toggle>();
        }
    }
}
