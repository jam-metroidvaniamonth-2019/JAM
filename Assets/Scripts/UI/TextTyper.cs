using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TextTyper : MonoBehaviour
    {
        [SerializeField] [TextArea] private string _displayText;
        [SerializeField] private float _characterDelay;
        [SerializeField] private Text _displayObject;

        public delegate void TypingCompleted();
        public TypingCompleted OnTypingCompleted;

        private bool _isTyping;
        private float _currentTypingTimer;
        private int _currentCharacterIndex;

        #region Unity Functions

        private void Update()
        {
            if (!_isTyping)
            {
                return;
            }

            _currentTypingTimer -= Time.deltaTime;
            if (_currentTypingTimer <= 0)
            {
                _displayObject.text += _displayText[_currentCharacterIndex];
                _currentTypingTimer = _characterDelay;

                _currentCharacterIndex += 1;
                if (_currentCharacterIndex >= _displayText.Length)
                {
                    StopTyping();
                }
            }
        }

        #endregion

        #region External Functions

        public void StartTyping()
        {
            _displayObject.text = "";
            _isTyping = true;
            _currentTypingTimer = 0;
            _currentCharacterIndex = 0;
        }

        public void ForceComplete()
        {
            _displayObject.text = _displayText;
            StopTyping();
        }

        public void UpdateText(string text) => _displayText = text;

        #endregion

        #region Utility Functions

        private void StopTyping()
        {
            _isTyping = false;
            NotifyTypingCompleted();
        }

        private void NotifyTypingCompleted() => OnTypingCompleted?.Invoke();

        #endregion
    }
}