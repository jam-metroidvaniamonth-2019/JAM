using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class Fader : MonoBehaviour
    {
        public delegate void FadeInComplete();
        public delegate void FadeOutComplete();

        public FadeInComplete OnFadeInComplete;
        public FadeOutComplete OnFadeOutComplete;

        [Header("Fade Rate")]
        [SerializeField] private float _fadeInRate;
        [SerializeField] private float _fadeOutRate;

        private Image _fadeImage;
        private bool _activateFadeIn;
        private bool _activateFadeOut;
        private float _currentAlpha;

        #region Unity Functions

        private void Start()
        {
            _fadeImage = GetComponent<Image>();
            _currentAlpha = ExtensionFunctions.Map(_fadeImage.color.a, 0, 1, 0, 255);
        }

        private void Update()
        {
            if (_activateFadeIn)
                FadeIn();
            else if (_activateFadeOut)
                FadeOut();
        }

        #endregion

        #region Utility Functions

        public void StartFadeIn()
        {
            _activateFadeIn = true;
            _activateFadeOut = false;
        }

        private void FadeIn()
        {
            _currentAlpha -= _fadeInRate * Time.deltaTime;

            Color fadeImageColor = _fadeImage.color;
            _fadeImage.color =
                ExtensionFunctions.ConvertAndClampColor(fadeImageColor.r, fadeImageColor.g, fadeImageColor.b,
                    _currentAlpha);

            if (!(_currentAlpha <= 0))
                return;

            OnFadeInComplete?.Invoke();
            _activateFadeIn = false;
            _fadeImage.gameObject.SetActive(false);
        }

        public void StartFadeOut()
        {
            _fadeImage.gameObject.SetActive(true);

            _activateFadeOut = true;
            _activateFadeIn = false;
        }

        private void FadeOut()
        {
            _currentAlpha += _fadeOutRate * Time.deltaTime;

            Color fadeImageColor = _fadeImage.color;
            _fadeImage.color =
                ExtensionFunctions.ConvertAndClampColor(fadeImageColor.r, fadeImageColor.g, fadeImageColor.b,
                    _currentAlpha);

            if (!(_currentAlpha >= 255))
                return;

            OnFadeOutComplete?.Invoke();
            _activateFadeOut = false;
        }

        #endregion

        #region Singleton

        public static Fader instance;

        private void Awake()
        {
            if (instance == null)
                instance = this;

            if (instance != this)
                Destroy(gameObject);
        }

        #endregion Singleton
    }
}