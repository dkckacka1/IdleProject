using System;
using UnityEngine;

namespace IdleProject.Core.UI
{
    public abstract class UIController : MonoBehaviour
    {
        protected virtual void Awake()
        {
            UIManager.Instance.AddUIController(this);
        }

        protected virtual void OnDestroy()
        {
            UIManager.Instance.RemoveUIController(this);
        }
    }
}
