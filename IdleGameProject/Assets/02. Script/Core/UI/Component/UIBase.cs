using UnityEngine;

namespace IdleProject.Core.UI
{
    public abstract class UIBase : MonoBehaviour
    {
        public new RectTransform RectTransform => transform as RectTransform; 
        
        protected virtual void Awake()
        {
            UIManager.Instance.AddUI(name, this);
        }

        protected void OnDestroy()
        {
            UIManager.Instance.RemoveUI(name);
        }
    }
}