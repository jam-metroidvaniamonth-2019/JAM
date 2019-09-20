﻿using System;
using System.Collections.Generic;
using Audio;
using Common;
using Player.Movement;
using Player.Shooting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Player.Display
{
    public class PlayerAnimator : MonoBehaviour
    {
        // Animation Constants
        private static readonly int JumpParam = Animator.StringToHash("Jump");
        private static readonly int FallParam = Animator.StringToHash("Fall");
        private static readonly int MoveParam = Animator.StringToHash("Move");
        private static readonly int DeadParam = Animator.StringToHash("Dead");
        private static readonly int BowIdleParam = Animator.StringToHash("Bow");
        private static readonly int SlingShotIdleParam = Animator.StringToHash("SlingShot");

        [Header("Controllers")]
        [SerializeField] private PlayerCollision _playerCollision;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private HealthSetter _playerHealthSetter;
        [SerializeField] private PlayerShooter _playerShooter;
        [SerializeField] private Rigidbody2D _playerRb;
        [SerializeField] private float _movementThreshold;
        public ParticleSystem _playerTrailParticleSystem;

        [Header("Player Animation Audio")]
        [SerializeField]
        private List<GameObject> _runningSounds;

        private Animator _playerAnimator;

        #region Unity Functions

        private void Start()
        {
            _playerAnimator = GetComponent<Animator>();

            _playerMovement.OnPlayerJumped += HandlePlayerJumped;
            _playerHealthSetter.OnHealthZero += HandlePlayerDead;

            _playerShooter.OnPlayerShootInPosition += HandlePlayerInShootPosition;
            _playerShooter.OnPlayerShot += HandlePlayerShot;
        }

        private void OnDestroy()
        {
            _playerMovement.OnPlayerJumped -= HandlePlayerJumped;
            _playerHealthSetter.OnHealthZero -= HandlePlayerDead;

            _playerShooter.OnPlayerShootInPosition -= HandlePlayerInShootPosition;
            _playerShooter.OnPlayerShot -= HandlePlayerShot;
        }

        private void LateUpdate()
        {
            _playerAnimator.SetBool(MoveParam, Math.Abs(_playerRb.velocity.x) > _movementThreshold);

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

            if (_playerCollision.IsOnGround && Math.Abs(_playerRb.velocity.x) > _movementThreshold)
            {
                if (_playerTrailParticleSystem.isStopped)
                {
                    _playerTrailParticleSystem.Play();
                }
            }
            else
            {
                if (_playerTrailParticleSystem.isPlaying)
                {
                    _playerTrailParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                }
            }
        }

        #endregion

        #region External Functions

        public void RevivePlayer()
        {
            _playerAnimator.SetBool(DeadParam, false);

            _playerAnimator.SetBool(JumpParam, false);
            _playerAnimator.SetBool(FallParam, false);

            _playerAnimator.SetBool(MoveParam, false);

            _playerAnimator.SetBool(BowIdleParam, false);
            _playerAnimator.SetBool(SlingShotIdleParam, false);
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

        private void HandlePlayerDead()
        {
            _playerAnimator.SetBool(DeadParam, true);

            _playerMovement.DisableMovement();
            _playerShooter.DisableShooting();
        }

        private void HandlePlayerInShootPosition(bool playerHasBow) =>
            _playerAnimator.SetBool(playerHasBow ? BowIdleParam : SlingShotIdleParam, true);

        private void HandlePlayerShot(bool playerHasBow, bool isValidShot) =>
            _playerAnimator.SetBool(playerHasBow ? BowIdleParam : SlingShotIdleParam, false);

        #endregion
    }
}