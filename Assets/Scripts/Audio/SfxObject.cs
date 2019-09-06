using UnityEngine;

namespace Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SfxObject : MonoBehaviour
    {
        private AudioSource _audioSource;
        private float _playDelay;

        #region Unity Functions

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Update()
        {
            if (_playDelay >= 0)
            {
                _playDelay -= Time.deltaTime;
            }

            if (_playDelay < 0 && !_audioSource.isPlaying)
            {
                Destroy(gameObject);
            }
        }

        #endregion

        #region External Functions

        public void PlayAudio(AudioClip audioClip, float playDelay)
        {
            _audioSource.clip = audioClip;
            _audioSource.PlayDelayed(playDelay);

            _playDelay = playDelay;
        }

        #endregion
    }
}