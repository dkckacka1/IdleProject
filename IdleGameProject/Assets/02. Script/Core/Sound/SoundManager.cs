using System.Collections.Generic;
using Engine.Util;
using IdleProject.Core.Resource;
using UnityEngine;
using UnityEngine.Serialization;

namespace IdleProject.Core.Sound
{
    public class SoundManager : SingletonMonoBehaviour<SoundManager>
    {
        [SerializeField]
        private AudioSource bgmSource;

        [SerializeField] private int defaultCreateSoundObjectCount = 10;
        
        [SerializeField] private SoundObject soundObjectPrefab;
        private readonly Queue<SoundObject> _sfxSoundObjectQueue = new();
        private AudioClip GetAudioClip(string clipName) => ResourceManager.Instance.GetAsset<AudioClip>(clipName);

        protected override void Initialized()
        {
            base.Initialized();

            for (int i = 0; i < defaultCreateSoundObjectCount; ++i)
            {
                _sfxSoundObjectQueue.Enqueue(CreateSoundObject());
            }
        }

        public void PlayBGM(string clipName)
        {
            var audioClip = GetAudioClip(clipName);
            bgmSource.clip = audioClip;
            bgmSource.Play();
        }

        public void PlaySfx(string clipName)
        {
            var audioClip = GetAudioClip(clipName);

            if (_sfxSoundObjectQueue.TryDequeue(out var soundObject) is false)
            {
                soundObject = CreateSoundObject();
            }
            
            soundObject.PlayAudio(audioClip, () =>
            {
                _sfxSoundObjectQueue.Enqueue(soundObject);
            });
        }

        private SoundObject CreateSoundObject()
        {
            return Instantiate(soundObjectPrefab, transform);
        }
    }
}
