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
        [SerializeField] private TextTyper _gameName;
        [SerializeField] private float _controlThreshold = 0.5f;
        [SerializeField] private TextFader[] _imageFaders;
        [SerializeField] private GameObject[] _underlines;

        private int _currentLineIndex;
        private bool _controllerDownState;

        #region Unity Functions

        private void Start()
        {
            _fader.OnFadeInComplete += HandleSceneFadeIn;
            _fader.OnFadeOutComplete += SwitchScene;

            _currentLineIndex = 0;
            _fader.StartFadeIn();
        }

        private void OnDestroy()
        {
            _fader.OnFadeOutComplete -= SwitchScene;
        }

        private void Update()
        {
            if (Input.GetButtonDown(ControlConstants.CloseButton) || Input.GetButtonDown(ControlConstants.BackButton))
            {
                HideControls();
            }

            float verticalY = Input.GetAxisRaw(ControlConstants.VerticalAxis);
            if (Mathf.Abs(verticalY) > _controlThreshold)
            {
                if (!_controllerDownState)
                {
                    _controllerDownState = true;

                    if (verticalY < 0)
                    {
                        _currentLineIndex += 1;
                    }
                    else if (verticalY > 0)
                    {
                        _currentLineIndex -= 1;
                        if (_currentLineIndex < 0)
                        {
                            _currentLineIndex = _underlines.Length + _currentLineIndex;
                        }
                    }

                    _currentLineIndex %= _underlines.Length;
                    _currentLineIndex = Mathf.Abs(_currentLineIndex);

                    UpdateControllerLines();
                }
            }
            else if (Mathf.Abs(verticalY) < _controlThreshold)
            {
                _controllerDownState = false;
            }

            if (Input.GetButtonDown(ControlConstants.StartButton) || Input.GetButtonDown(ControlConstants.SubmitButton))
            {
                switch (_currentLineIndex)
                {
                    case 0:
                        HandleStartPress();
                        break;

                    case 1:
                        DisplayControls();
                        break;

                    case 2:
                        QuitGame();
                        break;
                }
            }
        }

        #endregion

        #region External Functions

        public void HandleStartPress() => _fader.StartFadeOut();

        public void DisplayControls() => _controlsPanel.SetActive(true);

        public void QuitGame() => Application.Quit();

        #endregion

        #region Utility Functions

        private void UpdateControllerLines()
        {
            foreach (GameObject underline in _underlines)
            {
                underline.SetActive(false);
            }

            _underlines[_currentLineIndex].SetActive(true);
        }

        private void HandleSceneFadeIn()
        {
            _fader.OnFadeInComplete -= HandleSceneFadeIn;

            _gameName.StartTyping();
            _gameName.OnTypingCompleted += HandleTextTypingComplete;
        }

        private void HandleTextTypingComplete()
        {
            foreach (TextFader textFader in _imageFaders)
            {
                textFader.StartFadeOut();
            }

            _imageFaders[0].OnFadeOutComplete += DisplayUnderLine;
        }

        private void DisplayUnderLine()
        {
            _imageFaders[0].OnFadeOutComplete -= DisplayUnderLine;
            UpdateControllerLines();
        }

        private void HideControls() => _controlsPanel.SetActive(false);

        private void SwitchScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        #endregion
    }
}