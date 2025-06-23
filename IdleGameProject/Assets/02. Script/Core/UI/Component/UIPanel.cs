using System;
using Sirenix.OdinInspector;

namespace IdleProject.Core.UI
{
    public abstract class UIPanel : UIBase
    {
        protected abstract void Initialized();

        private void Start()
        {
            Initialized();
        }

        [Button]
        public virtual void OpenPanel()
        {
            gameObject.SetActive(true);
        }

        [Button]
        public virtual void ClosePanel()
        {
            gameObject.SetActive(false);
        }
    }
}