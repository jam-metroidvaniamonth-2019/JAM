using UnityEngine;

namespace Audio
{
    public class SfxAudioManager : MonoBehaviour
    {
        [SerializeField] private GameObject _audioPrefab;
        [SerializeField] private Transform _audioHolder;

        #region External Functions

        public void PlaySound(AudioClip audioClip, float playDelay)
        {
            GameObject audioInstance = Instantiate(_audioPrefab, _audioHolder.position, Quaternion.identity);
            audioInstance.transform.SetParent(_audioHolder);

            SfxObject sfxObject = audioInstance.GetComponent<SfxObject>();
            sfxObject.PlayAudio(audioClip, playDelay);
        }

        public void PlaySound(AudioClip audioClip, float playDelay, Vector3 worldPosition)
        {
            GameObject audioInstance = Instantiate(_audioPrefab, _audioHolder.position, Quaternion.identity);
            audioInstance.transform.SetParent(_audioHolder);
            audioInstance.transform.position = worldPosition;

            SfxObject sfxObject = audioInstance.GetComponent<SfxObject>();
            sfxObject.PlayAudio(audioClip, playDelay, true);
        }

        #endregion

        #region Singleton

        private static SfxAudioManager _instance;
        public static SfxAudioManager Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }

            if (_instance != this)
            {
                Destroy(gameObject);
            }

            DontDestroyOnLoad(gameObject);
        }

        #endregion
    }
}