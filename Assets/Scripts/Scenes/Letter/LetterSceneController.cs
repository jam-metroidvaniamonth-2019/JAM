using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Scenes.Letter
{
    public class LetterSceneController : MonoBehaviour
    {
        [SerializeField] private Fader _fader;
        [SerializeField] private float _faderDelay;
        [SerializeField] private TextTyper _letterTyper;
        [SerializeField] private Text _controlDisplayText;

        private bool _sceneExitTriggered;
        private float _triggerTimer;

        #region Unity Functions

        private void Start()
        {
            _fader.OnFadeInComplete += HandleFadeInComplete;
            _fader.OnFadeOutComplete += HandleFadeOutComplete;
            _letterTyper.OnTypingCompleted += HandleLetterTypingComplete;

            _fader.StartFadeIn();
        }

        private void Update()
        {
            UpdateTimer();
            CheckGamePadConnected();
            HandleForceSceneSwitch();
        }

        private void OnDestroy()
        {
            _fader.OnFadeInComplete -= HandleFadeInComplete;
            _fader.OnFadeOutComplete -= HandleFadeOutComplete;
            _letterTyper.OnTypingCompleted -= HandleLetterTypingComplete;
        }

        #endregion

        #region Utility Functions

        private void UpdateTimer()
        {
            if (_triggerTimer > 0)
            {
                _triggerTimer -= Time.deltaTime;
                if (_triggerTimer <= 0)
                {
                    if (!_sceneExitTriggered)
                    {
                        _letterTyper.StartTyping();
                    }
                    else
                    {
                        _fader.StartFadeOut();
                    }
                }
            }
        }

        private void HandleForceSceneSwitch()
        {
            if (_sceneExitTriggered)
            {
                return;
            }

            if (Input.GetButtonDown(ControlConstants.StartButton))
            {
                _letterTyper.ForceComplete();
            }
        }

        private void CheckGamePadConnected()
        {
            string[] connectedGamePads = Input.GetJoystickNames();
            if (connectedGamePads.Length > 0)
            {
                _controlDisplayText.text = string.IsNullOrEmpty(connectedGamePads[0])
                    ? "PRESS SPACE TO SKIP"
                    : "PRESS A TO SKIP";
            }
            else
            {
                _controlDisplayText.text = "PRESS SPACE TO SKIP";
            }
        }

        private void HandleFadeInComplete() => _triggerTimer = _faderDelay;

        private void HandleFadeOutComplete() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        private void HandleLetterTypingComplete()
        {
            _sceneExitTriggered = true;
            _triggerTimer = _faderDelay;
        }

        #endregion
    }
}