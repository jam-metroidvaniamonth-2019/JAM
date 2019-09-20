using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Common
{
    public class PauseAndResume : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;

        public delegate void PauseEnabled();
        public delegate void PauseDisabled();

        public PauseEnabled OnPauseEnabled;
        public PauseDisabled OnPauseDisabled;

        private void Start() => _pauseMenu.SetActive(false);

        public void PauseGame()
        {
            OnPauseEnabled?.Invoke();

            _pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }

        public void ResumeGame()
        {
            _pauseMenu.SetActive(false);
            Time.timeScale = 1;

            OnPauseDisabled?.Invoke();
        }

        public void QuitToMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        #region Singleton

        private static PauseAndResume _instance;
        public static PauseAndResume Instance => _instance;

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
        }

        #endregion Singleton
    }
}