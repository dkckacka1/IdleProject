using IdleProject.Core.Resource;
using UnityEngine;
using UnityEngine.UI;

namespace  IdleProject.Core.UI
{
    public class SlotUI : MonoBehaviour
    {

        [SerializeField] private Image slotFrameImage;
        [SerializeField] private Image slotBackground;
        [SerializeField] private Image slotImage;

        private Data.Data _data;
        
        public T GetData<T>() where T : Data.Data => _data as T;
        public void SetData<T>(T data) where T:  Data.Data, ISlotData
        {
            _data = data;
            var iconName = data.GetIconName;
            slotImage.sprite = ResourceManager.Instance.GetAsset<Sprite>(iconName);
        }

    }
}