using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace Scenes.Start
{
    public class StartSceneController : MonoBehaviour
    {
        [SerializeField] private Fader _fader;

        [Header("Input Detection")]
        [SerializeField] private string _gamePadText;
        [SerializeField] private string _keyboardText;
        [SerializeField] private TextTyper _inputTextTyper;

        [Header("GameName")]
        [SerializeField] private TextTyper _gameNameTyper;

        private bool _sceneExitTriggered;

        private bool _sceneActive;
        private bool _keyboardConnectedLastState;
        private bool _gamePadConnectedLastState;

        #region Unity Functions

        private void Start()
        {
            _fader.OnFadeOutComplete += SwitchScene;
            _fader.OnFadeInComplete += HandleFadeIn;

            _fader.StartFadeIn();
        }

        private void Update()
        {
            if (_sceneActive)
            {
                CheckGamePadConnected();
                ActivateSceneSwitch();
            }
        }

        private void OnDestroy()
        {
            _fader.OnFadeOutComplete -= SwitchScene;
        }

        #endregion

        #region Utility Functions

        private void ActivateSceneSwitch()
        {
            if (_sceneExitTriggered)
            {
                return;
            }

            if (Input.GetButtonDown(ControlConstants.StartButton))
            {
                _sceneExitTriggered = true;
                _fader.StartFadeOut();
            }
        }

        private void CheckGamePadConnected()
        {
            string[] connectedGamePads = Input.GetJoystickNames();
            if (connectedGamePads.Length > 0 && !string.IsNullOrEmpty(connectedGamePads[0]))
            {
                if (!_gamePadConnectedLastState)
                {
                    _inputTextTyper.UpdateText(_gamePadText);
                    _inputTextTyper.StartTyping();

                    _gamePadConnectedLastState = true;
                    _keyboardConnectedLastState = false;
                }
            }
            else
            {
                if (!_keyboardConnectedLastState)
                {
                    _inputTextTyper.UpdateText(_keyboardText);
                    _inputTextTyper.StartTyping();

                    _gamePadConnectedLastState = false;
                    _keyboardConnectedLastState = true;
                }
            }
        }

        private void HandleFadeIn()
        {
            _fader.OnFadeInComplete -= HandleFadeIn;

            _gameNameTyper.StartTyping();

            _inputTextTyper.UpdateText(_keyboardText);
            _inputTextTyper.StartTyping();

            _sceneActive = true;
        }

        private void SwitchScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        #endregion
    }
}