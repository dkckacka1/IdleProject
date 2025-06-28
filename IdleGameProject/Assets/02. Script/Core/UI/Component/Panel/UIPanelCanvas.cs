using UnityEngine;

namespace IdleProject.Core.UI
{
    public abstract class UIPanelCanvas : UIPanel
    {
        [HideInInspector] public Canvas canvas;

        protected override void Awake()
        {
            base.Awake();
            canvas = GetComponent<Canvas>();
        }

        public override void OpenPanel()
        {
            canvas.enabled = true;
        }

        public override void ClosePanel()
        {
            canvas.enabled = false;
        }
    }
}