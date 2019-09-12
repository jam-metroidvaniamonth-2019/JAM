using Audio;
using Player.Movement;
using Player.Shooting;
using UnityEngine;

namespace Player.Display
{
    public class PlayerAudioManager : MonoBehaviour
    {
        [Header("Event Emitters")]
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerCollision _playerCollision;
        [SerializeField] private PlayerShooter _playerShooting;
        [SerializeField] private Rigidbody2D _playerRb;

        [Header("Audio")]
        [SerializeField] private GameObject _playerLandedClip;
        [SerializeField] private GameObject _playJumpAudioPrefab;
        [SerializeField] private GameObject _playerDashClip;
        [SerializeField] private GameObject _playerShotBowArrowClip;
        [SerializeField] private GameObject _playerSlingShotClip;

        #region Unity Functions

        private void Start()
        {
            _playerShooting.OnPlayerShot += HandlePlayShooting;
            _playerCollision.OnGroundStatusChanged += HandlePlayerLanded;

            _playerMovement.OnPlayerJumped += HandlePlayerJumped;
            _playerMovement.OnPlayerDashed += HandlePlayerDashed;
        }

        #endregion

        #region Utility Functions

        private void HandlePlayShooting(bool playerHasBow) =>
            SfxAudioManager.Instance.PlaySound(playerHasBow ? _playerShotBowArrowClip : _playerSlingShotClip);

        private void HandlePlayerJumped()
        {
            SfxAudioManager.Instance.PlaySound(_playJumpAudioPrefab);
        }

        private void HandlePlayerLanded(bool playerLanded)
        {
            if (playerLanded)
            {
                SfxAudioManager.Instance.PlaySound(_playerLandedClip);
            }
        }

        private void HandlePlayerDashed()
        {
            SfxAudioManager.Instance.PlaySound(_playerDashClip);
        }

        #endregion
    }
}