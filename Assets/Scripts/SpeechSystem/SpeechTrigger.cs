using UnityEngine;
using Utils;

namespace SpeechSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class SpeechTrigger : MonoBehaviour
    {
        [SerializeField] [TextArea] private string[] _dialogues;

        private bool _speechActivated;

        #region Unity Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player) && !_speechActivated)
            {
                SimpleSpeechController.Instance.DisplayDialogues(_dialogues);
                _speechActivated = true;
            }
        }

        #endregion

        #region External Functions

        public void DisableSpeechTrigger() => _speechActivated = true;

        public void ReActiveSpeechTrigger() => _speechActivated = false;

        #endregion
    }
}