using System;
using UnityEngine;

namespace IdleProject.Core.UI
{
    public class UIPopup : UIBase
    {
        protected override void Awake()
        {
            base.Awake();
        }

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public virtual void OpenPopup()
        {
            gameObject.SetActive(true);
        }

        public virtual void ClosePopup()
        {
            
        }
    }
}