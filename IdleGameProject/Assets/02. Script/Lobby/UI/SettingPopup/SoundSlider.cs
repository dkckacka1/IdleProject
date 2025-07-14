using IdleProject.Core.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace IdleProject.Lobby.UI.SettingPopup
{
    public class SoundSlider : UIBase, IUIInit
    {
        [SerializeField] private Slider soundSlider;
        [SerializeField] private Image soundIconImage;

        [SerializeField] private Sprite muteSprite;
        [SerializeField] private Sprite defaultSprite;

        public void Initialized()
        {
            soundSlider.onValueChanged.AddListener(CheckMute);
        }

        private void CheckMute(float value)
        {
            soundIconImage.sprite = value is 0f ? muteSprite : defaultSprite;
        }

        public void SetSliderValueChangeEvent(UnityAction<float> onValueChanged)
        {
            soundSlider.onValueChanged.AddListener(onValueChanged);
        }

        public void SetSlider(float value)
        {
            soundSlider.value = value;
            CheckMute(value);
        }
    }
}   