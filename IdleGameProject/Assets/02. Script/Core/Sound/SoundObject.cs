using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.Core.Sound
{
    public class SoundObject : MonoBehaviour
    {
        private AudioSource _source;

        private UnityAction _onPlayEnd;
        
        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void SetPlayEndEvent(UnityAction onPlayEnd)
        {
            _onPlayEnd = onPlayEnd;
        }

        public void PauseAudio()
        {
            _source.Pause();
            _onPlayEnd?.Invoke();
        }

        public void PlayAudio(AudioClip audioClip, float volume = 1f)
        {
            _source.volume = volume;
            _source.clip = audioClip;
            _source.Play();
            _source.ObserveEveryValueChanged(source => source.isPlaying)
                .Pairwise()
                .Where(pair => pair is { Previous: true, Current: false })
                .Take(1) // 한 번만 실행되도록 제한
                .Subscribe(_ =>
                {
                    _onPlayEnd?.Invoke();
                });
        }
    }
}