using UnityEngine;

namespace Audio
{
    public class MusicManager : MonoBehaviour
    {
        [SerializeField] private GameObject _musicPrefab;
        [SerializeField] private Transform _musicHolder;

        #region External Functions

        public MusicObject PlaySound(AudioClip audioClip, float fadeInRate)
        {
            GameObject musicInstance = Instantiate(_musicPrefab, _musicHolder.position, Quaternion.identity);
            musicInstance.transform.SetParent(_musicHolder);

            MusicObject musicObject = musicInstance.GetComponent<MusicObject>();
            musicObject.PlayMusic(audioClip, fadeInRate);

            return musicObject;
        }

        public MusicObject PlaySound(GameObject musicPrefab, float fadeInRate)
        {
            GameObject audioInstance = Instantiate(musicPrefab, _musicHolder.position, Quaternion.identity);
            audioInstance.transform.SetParent(_musicHolder);

            MusicObject musicObject = audioInstance.GetComponent<MusicObject>();
            musicObject.PlayMusic(fadeInRate);

            return musicObject;
        }

        #endregion

        #region Singleton

        private static MusicManager _instance;
        public static MusicManager Instance => _instance;

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