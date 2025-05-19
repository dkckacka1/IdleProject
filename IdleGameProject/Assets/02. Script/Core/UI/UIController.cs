using UnityEngine;

namespace IdleProject.Core.UI
{
    public abstract class UIController : MonoBehaviour
    {

        protected void Start()
        {
            UIManager.Instance.AddUIController(this);
        }

        protected void Destroy()
        {
            UIManager.Instance.RemoveUIController(this);
        }
    }
}
