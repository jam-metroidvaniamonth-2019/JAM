using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.Letter
{
    public class LetterSceneController : MonoBehaviour
    {
        [SerializeField] private Fader _fader;
        [SerializeField] private float _faderDelay;
        [SerializeField] private TextTyper _letterTyper1;

        [Header("Input Detection")]
        [SerializeField] private string _gamePadText;
        [SerializeField] private string _keyboardText;
        [SerializeField] private TextTyper _inputTextTyper;


        private bool _sceneExitTriggered;
        private float _sceneExitTimer;

        private bool _sceneActive;
        private bool _keyboardConnectedLastState;
        private bool _gamePadConnectedLastState;

        #region Unity Functions

        private void Start()
        {
            _fader.OnFadeInComplete += HandleFadeInComplete;
            _fader.OnFadeOutComplete += HandleFadeOutComplete;

            _letterTyper1.OnTypingCompleted += HandleInitialLetterTypingComplete;

            _fader.StartFadeIn();
        }

        private void Update()
        {
            if (_sceneActive)
            {
                CheckGamePadConnected();
            }

            UpdateTimer();
        }

        private void OnDestroy()
        {
            _fader.OnFadeInComplete -= HandleFadeInComplete;
            _fader.OnFadeOutComplete -= HandleFadeOutComplete;
        }

        #endregion

        #region Utility Functions

        private void UpdateTimer()
        {
            if (_sceneExitTimer > 0 && _sceneExitTriggered)
            {
                _sceneExitTimer -= Time.deltaTime;
                if (_sceneExitTimer <= 0)
                {
                    HandleSceneSwitch();
                }
            }
        }

        private void HandleSceneSwitch() => _fader.StartFadeOut();
        
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

        private void HandleFadeInComplete()
        {
            _sceneActive = true;
            _letterTyper1.StartTyping();
        }

        private void HandleFadeOutComplete() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        private void HandleInitialLetterTypingComplete()
        {
            _letterTyper1.OnTypingCompleted -= HandleInitialLetterTypingComplete;

            _sceneExitTriggered = true;
            _sceneExitTimer = _faderDelay;
            _sceneActive = false;
        }

        #endregion
    }
}