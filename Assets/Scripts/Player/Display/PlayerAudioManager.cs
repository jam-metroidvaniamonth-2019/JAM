using System;
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
        [SerializeField] private AudioClip _playerLandedClip;
        [SerializeField] private GameObject _playJumpAudioPrefab;
        [SerializeField] private AudioClip _playerShotBowArrowClip;
        [SerializeField] private AudioClip _playerSlingShotClip;
        [SerializeField] private AudioSource _playerRunning;

        private float _previousVelocityState;

        #region Unity Functions

        private void Start()
        {
            _playerShooting.OnPlayerShot += HandlePlayShooting;
            _playerCollision.OnGroundStatusChanged += HandlePlayerLanded;
            _playerMovement.OnPlayerJumped += HandlePlayerJumped;
        }

        private void Update()
        {
            if (_playerRb.velocity.x != 0 && _previousVelocityState == 0)
            {
                _playerRunning.Play();
            }
            else if (_playerRb.velocity.x == 0 && _previousVelocityState != 0)
            {
                _playerRunning.Stop();
            }

            _previousVelocityState = _playerRb.velocity.x;
        }

        #endregion

        #region Utility Functions

        private void HandlePlayShooting()
        {
            // TODO: Check if Bow/Arrow or SlingShot
            
            SfxAudioManager.Instance.PlaySound(_playerShotBowArrowClip);
        }

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

        #endregion
    }
}