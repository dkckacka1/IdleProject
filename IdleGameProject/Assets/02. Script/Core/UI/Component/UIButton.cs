using UnityEngine;
using UnityEngine.UI;


namespace IdleProject.Core.UI
{
    [RequireComponent(typeof(Button))]
    public class UIButton : UIBase
    {
        public Button Button { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Button = GetComponent<Button>();
        }
    }
}