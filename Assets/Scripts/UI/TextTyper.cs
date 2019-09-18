using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TextTyper : MonoBehaviour
    {
        [SerializeField] [TextArea] private string _displayText;
        [SerializeField] private float _characterDelay;

        [Header("Display Text (Use Either)")]
        [SerializeField] private Text _displayObject;
        [SerializeField] private TextMeshProUGUI _textMeshDisplayObject;

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
                AddText(_displayText[_currentCharacterIndex]);
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
            SetText("");
            _isTyping = true;
            _currentTypingTimer = 0;
            _currentCharacterIndex = 0;
        }

        public void ForceComplete()
        {
            SetText(_displayText);
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

        private void SetText(string text)
        {
            if (_displayObject != null)
            {
                _displayObject.text = text;
            }
            else
            {
                _textMeshDisplayObject.text = text;
            }
        }

        private void AddText(string text)
        {
            if (_displayObject != null)
            {
                _displayObject.text += text;
            }
            else
            {
                _textMeshDisplayObject.text += text;
            }
        }

        private void AddText(char character)
        {
            if (_displayObject != null)
            {
                _displayObject.text += character;
            }
            else
            {
                _textMeshDisplayObject.text += character;
            }
        }

        #endregion
    }
}