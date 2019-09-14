using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scenes.GameOver
{
    public class GameOverSceneController : MonoBehaviour
    {
        [SerializeField] private Fader _fader;
        [SerializeField] private float _waitTimer;

        [Header("GameOver Text")]
        [SerializeField] private TextTyper _gameOverTyper;
        [SerializeField] [TextArea] private string _gameOverWinText;
        [SerializeField] [TextArea] private string _gameOverLoseText;

        private bool _sceneActive;
        private float _currentCountdownTime;

        #region Unity Functions

        private void Start()
        {
            _fader.OnFadeInComplete += HandleSceneFadeIn;
            _fader.OnFadeOutComplete += SwitchScene;

            _fader.StartFadeIn();

            if (GameOverSceneData.didPlayerWin)
            {
                _gameOverTyper.UpdateText(_gameOverWinText);
            }
            else
            {
                _gameOverTyper.UpdateText(_gameOverLoseText);
            }
        }

        private void OnDestroy()
        {
            _fader.OnFadeInComplete -= HandleSceneFadeIn;
            _fader.OnFadeOutComplete -= SwitchScene;
        }

        private void Update()
        {
            if (!_sceneActive)
            {
                return;
            }

            _currentCountdownTime -= Time.deltaTime;

            if (_currentCountdownTime <= 0)
            {
                _fader.StartFadeOut();
                _sceneActive = false;
            }
        }

        #endregion

        #region Utility Functions

        private void HandleSceneFadeIn()
        {
            _currentCountdownTime = _waitTimer;
            _gameOverTyper.StartTyping();
            _gameOverTyper.OnTypingCompleted += HandleTypingComplete;
        }

        private void HandleTypingComplete()
        {
            _gameOverTyper.OnTypingCompleted -= HandleTypingComplete;
            _sceneActive = true;
        }

        private void SwitchScene() => SceneManager.LoadScene(0);

        #endregion
    }
}