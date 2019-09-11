using UnityEngine;
using Utils;

namespace SpeechSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class SpeechTrigger : MonoBehaviour
    {
        [SerializeField] [TextArea] private string[] _dialogues;

        private bool _speechActivated;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player) && !_speechActivated)
            {
                SimpleSpeechController.Instance.DisplayDialogues(_dialogues);
                _speechActivated = true;
            }
        }
    }
}