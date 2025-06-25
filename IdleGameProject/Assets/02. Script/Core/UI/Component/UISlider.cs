using UnityEngine;
using UnityEngine.UI;

namespace IdleProject.Core.UI
{
    [RequireComponent(typeof(Slider))]
    public class UISlider : UIBase
    {
        public Slider Slider { get; private set; }
        
        protected override void Awake()
        {
            base.Awake();
            Slider = GetComponent<Slider>();
        }
    }
}

