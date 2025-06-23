using Sirenix.OdinInspector;

namespace IdleProject.Core.UI
{
    public abstract class UIPopup : UIBase
    {
        public abstract void Initialized();
        
        [Button]
        public virtual void OpenPopup()
        {
            gameObject.SetActive(true);
        }

        [Button]
        public virtual void ClosePopup()
        {
            gameObject.SetActive(false);
        }
    }
}