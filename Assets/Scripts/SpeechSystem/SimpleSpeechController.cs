using UI;
using UnityEngine;

namespace SpeechSystem
{
    public class SimpleSpeechController : MonoBehaviour
    {
        [SerializeField] private Animator _speechBubbleAnimator;
        [SerializeField] private TextTyper _speechTextTyper;
        [SerializeField] private float _timeBetweenMultipleDialogues;

        public delegate void SpeechBubbleComplete();
        public SpeechBubbleComplete OnSpeechBubbleComplete;

        private string[] _currentDialogues;
        private bool _dialogueActive;
        private bool _dialogueComplete;
        private float _dialogueTimerWait;
        private int _currentDialogueIndex;

        private static readonly int DialogueDisplayParam = Animator.StringToHash("DialogueDisplay");

        #region Unity Functions

        private void Start() => _speechTextTyper.OnTypingCompleted += HandleTextTypingComplete;

        private void OnDestroy() => _speechTextTyper.OnTypingCompleted -= HandleTextTypingComplete;

        private void Update()
        {
            if (!_dialogueActive)
            {
                return;
            }

            if (_dialogueTimerWait > 0 && _dialogueComplete)
            {
                _dialogueTimerWait -= Time.deltaTime;

                if (_dialogueTimerWait <= 0)
                {
                    CheckAndSwitchNextDialogue();
                }
            }
        }

        #endregion

        #region External Functions

        public void DisplayDialogues(string[] dialogues)
        {
            _currentDialogues = dialogues;
            _currentDialogueIndex = 0;
            _dialogueActive = true;
            _dialogueTimerWait = _timeBetweenMultipleDialogues;
            _dialogueComplete = false;

            _speechTextTyper.UpdateText(_currentDialogues[_currentDialogueIndex]);
            _speechTextTyper.StartTyping();

            _speechBubbleAnimator.SetBool(DialogueDisplayParam, true);
        }

        public void StopAndClearDialogues()
        {
            _dialogueActive = false;
            _speechBubbleAnimator.SetBool(DialogueDisplayParam, false);

            OnSpeechBubbleComplete?.Invoke();
        }

        #endregion

        #region Utility Functions

        private void CheckAndSwitchNextDialogue()
        {
            _currentDialogueIndex += 1;
            _dialogueComplete = false;

            if (_currentDialogueIndex >= _currentDialogues.Length)
            {
                StopAndClearDialogues();
            }
            else
            {
                _speechTextTyper.UpdateText(_currentDialogues[_currentDialogueIndex]);
                _speechTextTyper.StartTyping();
            }
        }

        private void HandleTextTypingComplete()
        {
            _dialogueTimerWait = _timeBetweenMultipleDialogues;
            _dialogueComplete = true;
        }

        #endregion

        #region Singleton

        private static SimpleSpeechController _instance;
        public static SimpleSpeechController Instance => _instance;

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