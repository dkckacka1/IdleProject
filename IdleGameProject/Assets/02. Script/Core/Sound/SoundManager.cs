using System;
using System.Collections.Generic;
using System.Linq;
using Engine.Util;
using IdleProject.Core.GameData;
using IdleProject.Core.Resource;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace IdleProject.Core.Sound
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [SerializeField]
        private AudioSource bgmSource;

        [SerializeField] private SoundObject soundObjectPrefab;
        [SerializeField] private AudioMixerGroup sfxAudioMixerGroup;
        
        private readonly Queue<SoundObject> _sfxSoundObjectQueue = new();
        private readonly List<SoundObject> _playingSoundObjectList = new();
        private readonly HashSet<string> _sfxPlaySet = new(); // 한 프레임 내에 같은 이름의 SFX 호출 방지  
        private AudioClip GetAudioClip(string clipName) => ResourceManager.Instance.GetAsset<AudioClip>(clipName);

        protected override void Initialized()
        {
            base.Initialized();

            var createCount = DataManager.Instance.ConstData.defaultCreateSoundObjectCount;
            for (int i = 0; i < createCount; ++i)
            {
                _sfxSoundObjectQueue.Enqueue(CreateSoundObject());
            }

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void LateUpdate()
        {
            _sfxPlaySet.Clear();
        }

        public void PlayBGM(string clipName)
        {
            var audioClip = GetAudioClip(clipName);
            if (audioClip is null) return;
            
            bgmSource.clip = audioClip;
            bgmSource.Play();
        }

        public void PauseBGM()
        {
            bgmSource.Pause();
        }

        public void PlaySfx(string clipName)
        {
            if (_sfxPlaySet.Contains(clipName)) return;
            
            var audioClip = GetAudioClip(clipName);
            if (audioClip is not null)
            {
                _sfxPlaySet.Add(clipName);
            
                if (_sfxSoundObjectQueue.TryDequeue(out var soundObject) is false)
                {
                    soundObject = CreateSoundObject();
                }
            
                soundObject.PlayAudio(audioClip);
                _playingSoundObjectList.Add(soundObject);
            }
        }

        private SoundObject CreateSoundObject()
        {
            var soundObject = Instantiate(soundObjectPrefab, transform);
            soundObject.SetMixerGroup(sfxAudioMixerGroup);
            soundObject.SetPlayEndEvent(() => PlayEndSoundObject(soundObject));
            
            return soundObject;
        }

        private void PlayEndSoundObject(SoundObject soundObject)
        {
            _sfxSoundObjectQueue.Enqueue(soundObject);
            _playingSoundObjectList.Remove(soundObject);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            for (int i = _playingSoundObjectList.Count - 1; i >= 0; i--)
            {
                _playingSoundObjectList[i].PauseAudio();
            }
        }
    }
}
