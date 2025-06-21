using System;
using UnityEngine;

namespace IdleProject.Core.UI
{
    public abstract class UIPopup : UIBase
    {
        public abstract void Initialized();
        
        public virtual void OpenPopup()
        {
            gameObject.SetActive(true);
        }

        public virtual void ClosePopup()
        {
            gameObject.SetActive(false);
        }
    }
}