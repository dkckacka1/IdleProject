using System;
using UnityEngine;

namespace IdleProject.Core.UI
{
    public abstract class UIBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            UIManager.Instance.AddUI(name, this);
            Debug.Log(name);
        }

        protected void OnDestroy()
        {
            UIManager.Instance.RemoveUI(name);
        }
    }
}