using Sirenix.OdinInspector;

namespace IdleProject.Core.UI
{
    public abstract class UIPanel : UIBase, IUIInit
    {
        public abstract void Initialized();

        public PanelRadioGroup RadioGroup;
        public virtual bool IsOpened => gameObject.activeInHierarchy;
        
        [Button]
        public virtual void OpenPanel()
        {
            OpenAction();
            RadioGroup?.OpenNotify(this);
        }

        [Button]
        public virtual void ClosePanel()
        {
            CloseAction();
        }

        protected virtual void OpenAction()
        {
            gameObject.SetActive(true);
        }
        
        protected virtual void CloseAction()
        {
            gameObject.SetActive(false);
        }
    }
}