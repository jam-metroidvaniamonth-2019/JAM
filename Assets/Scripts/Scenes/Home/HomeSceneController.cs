using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Scenes.Home
{
    public class HomeSceneController : MonoBehaviour
    {
        [SerializeField] private Fader _fader;
        [SerializeField] private GameObject _controlsPanel;

        private bool _controlPanelOpen;

        #region Unity Functions

        private void Start()
        {
            _fader.OnFadeOutComplete += SwitchScene;
            _fader.StartFadeIn();
        }

        private void OnDestroy()
        {
            _fader.OnFadeOutComplete -= SwitchScene;
        }

        private void Update()
        {
            if (Input.GetButtonDown(ControlConstants.CloseButton))
            {
                HideControls();
            }
        }

        #endregion

        #region External Functions

        public void HandleStartPress() => _fader.StartFadeOut();

        public void DisplayControls() => _controlsPanel.SetActive(true);

        public void QuitGame() => Application.Quit();

        #endregion

        #region Utility Functions

        private void HideControls() => _controlsPanel.SetActive(false);

        private void SwitchScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        #endregion
    }
}