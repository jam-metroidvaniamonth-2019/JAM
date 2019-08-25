using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace Scenes.Start
{
    public class StartSceneController : MonoBehaviour
    {
        [SerializeField] private Fader _fader;

        [Header("Input Detection")]
        [SerializeField] private Image _gamePadKeyboardImage;
        [SerializeField] private Sprite _gamePadSprite;
        [SerializeField] private Sprite _keyboardSprite;

        private bool _sceneExitTriggered;

        #region Unity Functions

        private void Start()
        {
            _fader.OnFadeOutComplete += SwitchScene;
            _fader.StartFadeIn();
        }

        private void Update()
        {
            CheckGamePadConnected();
            ActivateSceneSwitch();
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
            if (connectedGamePads.Length > 0)
            {
                _gamePadKeyboardImage.sprite =
                    string.IsNullOrEmpty(connectedGamePads[0]) ? _keyboardSprite : _gamePadSprite;
            }
            else
            {
                _gamePadKeyboardImage.sprite = _keyboardSprite;
            }
        }

        private void SwitchScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        #endregion
    }
}