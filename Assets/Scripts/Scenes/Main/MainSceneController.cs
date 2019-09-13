using Audio;
using Scenes.Common;
using UI;
using UI.CutScene;
using UnityEngine;
using Utils;

namespace Scenes.Main
{
    public class MainSceneController : MonoBehaviour
    {
        [SerializeField] private PauseAndResume _pauseAndResume;
        [SerializeField] private Fader _fader;

        [Header("Background Music")]
        [SerializeField] private GameObject _backgroundMusicPrefab;
        [SerializeField] private float _audioFadeInRate = 0.15f;

        private CutSceneDisplay _cutSceneDisplay;

        private bool _gamePaused;
        private bool _cutSceneOpen;

        #region Unity Functions

        private void Start()
        {
            _cutSceneDisplay = CutSceneDisplay.Instance;
            _cutSceneDisplay.OnCutSceneOpen += HandleCutSceneOpen;
            _cutSceneDisplay.OnCutSceneClose += HandleCutSceneClose;

            _pauseAndResume.OnPauseDisabled += HandleGameUnPaused;

            MusicManager.Instance.PlaySound(_backgroundMusicPrefab, _audioFadeInRate);

            _fader.StartFadeIn();
        }

        private void OnDestroy()
        {
            _cutSceneDisplay.OnCutSceneOpen -= HandleCutSceneOpen;
            _cutSceneDisplay.OnCutSceneClose -= HandleCutSceneClose;

            _pauseAndResume.OnPauseDisabled -= HandleGameUnPaused;
        }

        private void Update()
        {
            if (!Input.GetButtonDown(ControlConstants.CloseButton))
            {
                return;
            }

            if (_cutSceneOpen)
            {
                _cutSceneDisplay.CloseCutScene();
            }
            else
            {
                if (!_gamePaused)
                {
                    OpenPauseMenu();
                }
                else
                {
                    _pauseAndResume.ResumeGame();
                }
            }
        }

        #endregion

        #region Utility Functions

        private void OpenPauseMenu()
        {
            _gamePaused = true;
            _pauseAndResume.PauseGame();
        }

        private void HandleGameUnPaused() => _gamePaused = false;

        private void HandleCutSceneOpen() => _cutSceneOpen = true;

        private void HandleCutSceneClose() => _cutSceneOpen = false;

        #endregion
    }
}