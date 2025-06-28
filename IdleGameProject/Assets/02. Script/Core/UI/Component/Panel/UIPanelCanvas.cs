using UnityEngine;

namespace IdleProject.Core.UI
{
    public abstract class UIPanelCanvas : UIPanel
    {
        [HideInInspector] public Canvas canvas;
        public override bool IsOpened => canvas.enabled;

        protected override void Awake()
        {
            base.Awake();
            canvas = GetComponent<Canvas>();
        }

        protected override void OpenAction()
        {
            canvas.enabled = true;
        }

        protected override void CloseAction()
        {
            canvas.enabled = false;
        }
    }
}