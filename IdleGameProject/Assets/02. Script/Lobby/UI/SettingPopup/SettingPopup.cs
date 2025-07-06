using IdleProject.Core.GameData;
using IdleProject.Core.Sound;
using IdleProject.Core.UI;
using IdleProject.Lobby.UI;
using IdleProject.Lobby.UI.SettingPopup;
using UnityEditor;
using UnityEngine;

namespace IdleProject
{
    public class SettingPopup : UIPopup
    {
        private SoundSlider _bgmSlider;
        private SoundSlider _sfxSlider;
        
        public override void Initialized()
        {
            base.Initialized();

            _bgmSlider = UIManager.Instance.GetUI<SoundSlider>("BGMSoundSlider");
            _bgmSlider.SetSlider(PreferenceData.BgmSoundVolume);
            _bgmSlider.SetSliderValueChangeEvent(value => PreferenceData.BgmSoundVolume = value);
            
            _sfxSlider = UIManager.Instance.GetUI<SoundSlider>("SFXSoundSlider");
            _sfxSlider.SetSlider(PreferenceData.SfxSoundVolume);
            _sfxSlider.SetSliderValueChangeEvent(value => PreferenceData.SfxSoundVolume = value);
            _sfxSlider.SetSliderValueChangeEvent(_ => SoundManager.Instance.PlaySfx("ShieldSoftBlue"));

            UIManager.Instance.GetUI<UIButton>("SettingPopupExitButton").Button.onClick.AddListener(ExitGame);
        }

        private void ExitGame()
        {
            #if UNITY_EDITOR
            UIManager.Instance.OpenConfirmPopup("게임을 종료 하시겠습니까?", EditorApplication.ExitPlaymode);
            #else 
            UIManager.Instance.OpenConfirmPopup("게임을 종료 하시겠습니까?", Application.Quit);
            #endif
        }
    }
}