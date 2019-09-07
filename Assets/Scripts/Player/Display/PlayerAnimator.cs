﻿using System.Collections.Generic;
using Audio;
using Player.Movement;
using UnityEngine;

namespace Player.Display
{
    public class PlayerAnimator : MonoBehaviour
    {
        // Animation Constants
        private static readonly int JumpParam = Animator.StringToHash("Jump");
        private static readonly int FallParam = Animator.StringToHash("Fall");
        private static readonly int MoveParam = Animator.StringToHash("Move");

        [Header("Controllers")]
        [SerializeField] private PlayerCollision _playerCollision;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private Rigidbody2D _playerRb;
        [SerializeField] private float _movementThreshold;

        [Header("Player Animation Audio")]
        [SerializeField] private List<GameObject> _runningSounds;

        private Animator _playerAnimator;

        #region Unity Functions

        private void Start()
        {
            _playerAnimator = GetComponent<Animator>();
            _playerMovement.OnPlayerJumped += HandlePlayerJumped;
        }

        private void Update()
        {
            _playerAnimator.SetBool(MoveParam, _playerRb.velocity.x != 0);

            // This extra check is added as sometimes
            // when colliding with walls and then falling
            // the player can have an incorrect animation state
            if (_playerRb.velocity.y > -_movementThreshold && _playerRb.velocity.y < _movementThreshold)
            {
                _playerAnimator.SetBool(JumpParam, false);
                _playerAnimator.SetBool(FallParam, false);
            }

            if (_playerCollision.IsOnGround)
            {
                _playerAnimator.SetBool(FallParam, false);
                _playerAnimator.SetBool(JumpParam, false);
            }
            else if (_playerRb.velocity.y < -_movementThreshold)
            {
                _playerAnimator.SetBool(JumpParam, false);
                _playerAnimator.SetBool(FallParam, true);
            }
        }

        #endregion

        #region Animation Control

        private void HandlePlayerJumped()
        {
            _playerAnimator.SetBool(JumpParam, true);
            _playerAnimator.SetBool(FallParam, false);
        }

        private void PlayRandomRunningSound()
        {
            GameObject audioPrefab = _runningSounds[Mathf.FloorToInt(Random.value * _runningSounds.Count)];
            SfxAudioManager.Instance.PlaySound(audioPrefab);
        }

        #endregion
    }
}