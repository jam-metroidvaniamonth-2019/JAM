using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Image))]
    public class ImageFader : MonoBehaviour
    {
        [SerializeField] private float _fadeRate;

        private bool _isFadingActive;
        private bool _isFadingIn;
        private float _currentAlpha;

        private Image _affectorImage;

        #region Unity Functions

        private void Start()
        {
            _affectorImage = GetComponent<Image>();
            _isFadingActive = true;
            _isFadingIn = false;
        }

        private void Update()
        {
            if (!_isFadingActive)
            {
                return;
            }

            if (!_isFadingIn)
            {
                Color fadeOutColor = _affectorImage.color;
                fadeOutColor.a = _currentAlpha;
                _affectorImage.color = fadeOutColor;

                _currentAlpha -= Time.deltaTime * _fadeRate;
                if (_currentAlpha <= 0)
                {
                    _isFadingIn = true;
                    _currentAlpha = 0;
                }
            }
            else
            {
                Color fadeInColor = _affectorImage.color;
                fadeInColor.a = _currentAlpha;
                _affectorImage.color = fadeInColor;

                _currentAlpha += _fadeRate * Time.deltaTime;
                if (_currentAlpha >= 1)
                {
                    _isFadingIn = false;
                    _currentAlpha = 1;
                }
            }
        }

        #endregion

        #region External Functions

        public void StartFading() => _isFadingActive = true;

        public void StopFading()
        {
            _isFadingActive = false;
        }

        #endregion
    }
}