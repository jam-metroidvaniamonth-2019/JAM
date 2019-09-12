using Player.Movement;
using Player.Shooting;
using UnityEngine;
using Utils;

namespace SpeechSystem
{
    [RequireComponent(typeof(Collider2D))]
    public class SpeechTrigger : MonoBehaviour
    {
        [SerializeField] [TextArea] private string[] _dialogues;

        private bool _speechActivated;

        private PlayerMovement _playerMovement;
        private PlayerShooter _playerShooter;

        #region Unity Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player) && !_speechActivated)
            {
                SimpleSpeechController.Instance.DisplayDialogues(_dialogues);
                _speechActivated = true;

                _playerMovement = other.GetComponent<PlayerMovement>();
                _playerShooter = other.GetComponent<PlayerShooter>();

                DisablePlayerControls();

                SimpleSpeechController.Instance.OnSpeechBubbleComplete += HandleSpeechBubbleClose;
            }
        }

        #endregion

        #region Utility Functions

        private void HandleSpeechBubbleClose()
        {
            SimpleSpeechController.Instance.OnSpeechBubbleComplete -= HandleSpeechBubbleClose;
            EnablePlayerControls();
        }

        private void DisablePlayerControls()
        {
            _playerMovement.DisableMovement();
            _playerShooter.DisableShooting();
        }

        private void EnablePlayerControls()
        {
            _playerMovement.EnableMovement();
            _playerShooter.EnableShooting();
        }

        #endregion
    }
}