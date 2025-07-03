using UniRx;
using UnityEngine;
using UnityEngine.Events;

namespace IdleProject.Core.Sound
{
    public class SoundObject : MonoBehaviour
    {
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        public void PlayAudioLoop(AudioClip audioClip)
        {
            _source.clip = audioClip;
        }

        public void PlayAudio(AudioClip audioClip, UnityAction onPlayEnd = null)
        {
            _source.PlayOneShot(audioClip);

            _source.ObserveEveryValueChanged(source => source.isPlaying).Where(isPlaying => false).Subscribe(_ =>
            {
                onPlayEnd?.Invoke();
            });
        }
    }
}