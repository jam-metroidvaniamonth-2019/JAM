using System;
using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicObject : MonoBehaviour
    {
        private AudioSource _audioSource;

        private float _audioChangeRate;
        private bool _isFadingIn;

        #region Unity Functions

        private void Start() => _audioSource = GetComponent<AudioSource>();

        private void Update()
        {
            if (_isFadingIn && _audioSource.volume < 1)
            {
                _audioSource.volume += _audioChangeRate * Time.deltaTime;
            }

            else if (!_isFadingIn && _audioSource.volume > 0)
            {
                _audioSource.volume -= _audioChangeRate * Time.deltaTime;

                if (_audioSource.volume <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }

        #endregion

        #region External Functions

        public void PlayMusic(AudioClip audioClip, float fadeInRate)
        {
            _isFadingIn = true;
            _audioChangeRate = fadeInRate;

            _audioSource.volume = 0;
            _audioSource.clip = audioClip;
            _audioSource.Play();
        }

        public void StopMusic(float fadeOutRate)
        {
            _audioSource.volume = 1;
            _audioChangeRate = fadeOutRate;
            _isFadingIn = false;
        }

        #endregion
    }
}