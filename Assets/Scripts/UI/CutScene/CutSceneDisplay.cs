using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.CutScene
{
    public class CutSceneDisplay : MonoBehaviour
    {
        [SerializeField] private Fader _cutSceneFader;
        [SerializeField] private Fader _cutSceneBackgroundFader;
        [SerializeField] private Image _cutSceneImage;

        public delegate void CutSceneOpen();
        public delegate void CutSceneClose();

        public CutSceneOpen OnCutSceneOpen;
        public CutSceneClose OnCutSceneClose;

        private bool _cutSceneOpen;
        private float _displayTimer;

        #region Unity Functions

        private void Update()
        {
            if (_cutSceneOpen)
            {
                _displayTimer -= Time.deltaTime;

                if (_displayTimer <= 0)
                {
                    CloseCutScene();
                }
            }
        }

        #endregion

        #region External Functions

        public void DisplayCutScene(Sprite displayImage, float displayTimer)
        {
            _displayTimer = displayTimer;
            _cutSceneImage.sprite = displayImage;

            if (!_cutSceneOpen)
            {
                _cutSceneFader.StartFadeOut(true);
                _cutSceneBackgroundFader.StartFadeOut(true);

                _cutSceneFader.OnFadeOutComplete += HandleCutSceneOpen;
            }
        }

        #endregion

        #region Utility Functions

        private void HandleCutSceneOpen()
        {
            _cutSceneFader.OnFadeOutComplete -= HandleCutSceneOpen;
            _cutSceneOpen = true;
            OnCutSceneOpen?.Invoke();
        }

        private void HandleCutSceneClose()
        {
            _cutSceneFader.OnFadeInComplete -= HandleCutSceneClose;
            OnCutSceneClose?.Invoke();
        }

        public void CloseCutScene()
        {
            if (!_cutSceneOpen)
            {
                return;
            }

            _cutSceneOpen = false;

            _cutSceneFader.StartFadeIn();
            _cutSceneBackgroundFader.StartFadeIn();

            _cutSceneFader.OnFadeInComplete += HandleCutSceneClose;
        }

        #endregion

        #region Singleton

        private static CutSceneDisplay _instance;
        public static CutSceneDisplay Instance => _instance;

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

        #endregion
    }
}