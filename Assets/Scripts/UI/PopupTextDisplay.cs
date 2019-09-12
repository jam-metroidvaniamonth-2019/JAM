using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PopupTextDisplay : MonoBehaviour
    {
        [SerializeField] private Text _displayText;
        [SerializeField] private Animator _displayTextAnimator;

        private static readonly int DisplayParam = Animator.StringToHash("DisplayParam");

        #region External Functions

        public void DisplayText(string text)
        {
            DisplayText(text, Color.black);
        }

        public void DisplayText(string text, Color color)
        {
            _displayText.text = text;
            _displayText.color = color;

            _displayTextAnimator.SetTrigger(DisplayParam);
        }

        #endregion

        #region Singleton

        private static PopupTextDisplay _instance;
        public static PopupTextDisplay Instance => _instance;

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