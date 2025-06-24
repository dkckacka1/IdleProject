using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace IdleProject.Core.UI
{
    public abstract class UIPanel : UIBase, IUIInit
    {
        public abstract UniTask Initialized();
        
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