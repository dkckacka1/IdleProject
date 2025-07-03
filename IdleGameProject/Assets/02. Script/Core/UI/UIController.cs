using Cysharp.Threading.Tasks;
using UnityEngine;

namespace IdleProject.Core.UI
{
    public abstract class UIController : MonoBehaviour
    {
        protected virtual void Awake()
        {
            UIManager.Instance.SetUIController(this);
        }

        public abstract UniTask Initialized();
    }
}
