using IdleProject.Core.Sound;
using UnityEngine;

namespace IdleProject.Core.GameData
{
    public static class PreferenceData
    {
        private const string BGM_SOUND_VALUE_KEY = "bgmSoundValueKey";
        private const string SFX_SOUND_VALUE_KEY = "sfxSoundValueKey";
        
        public static float BgmSoundVolume
        {
            get => PlayerPrefs.GetFloat(BGM_SOUND_VALUE_KEY, 1);
            set
            {
                SoundManager.Instance.ChangeBGMVolume(value);
                PlayerPrefs.SetFloat(BGM_SOUND_VALUE_KEY, value);
            }
        }

        public static float SfxSoundVolume
        {
            get => PlayerPrefs.GetFloat(SFX_SOUND_VALUE_KEY, 1);
            set
            {
                SoundManager.Instance.ChangeSfxVolume(value);
                PlayerPrefs.SetFloat(SFX_SOUND_VALUE_KEY, value);
            }
        }
    }
}