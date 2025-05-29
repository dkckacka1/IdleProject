using TMPro;
using UnityEngine;

namespace IdleProject.Core.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIText : UIBase
    {
        public TextMeshProUGUI Text { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Text = GetComponent<TextMeshProUGUI>();
        }
    }
}
