﻿using System.Collections;
using UnityEngine;
using Utils;

namespace Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _movementSpeed = 250;
        [SerializeField] private float _linearDrag = 50;
        [SerializeField] private float _linearDragThreasholdVelocity = 0.5f;
        [SerializeField] private SpriteRenderer _playerSprite;

        [Header("Jump")]
        [SerializeField] private float _jumpVelocity = 7;
        [SerializeField] private float _fallMultiplier = 5f;
        [SerializeField] private float _lowJumpMultiplier = 2;
        [SerializeField] private float _jumpDecelearationRate = 10;

        [Header("Dash")]
        [SerializeField] private float _dashSpeed = 500f;
        [SerializeField] private float _dashEffectTime = 0.3f;

        private Rigidbody2D _playerRb;
        private PlayerCollision _playerCollision;

        // Dash
        private bool _dashActive;
        private bool _dashUsed;

        // Jump
        private bool _jumped;

        // Delegates
        public delegate void PlayerJumped();
        public PlayerJumped OnPlayerJumped;

        #region Unity Functions

        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _playerCollision = GetComponent<PlayerCollision>();
        }

        private void Update()
        {
            UpdateMovementVariables();

            HandleDash();

            if (!_dashActive)
            {
                HandleHorizontalMovement();
                HandleJump();
            }
        }

        #endregion

        #region Movement

        #region Horizontal Movement

        private void HandleHorizontalMovement()
        {
            float moveX = Input.GetAxis(ControlConstants.HorizontalAxis);
            float moveXRaw = Input.GetAxisRaw(ControlConstants.HorizontalAxis);

            if (moveXRaw == 0 && _playerCollision.IsOnGround)
            {
                _playerRb.drag = _linearDrag;
            }
            else
            {
                _playerRb.drag = 0;
            }

            _playerRb.velocity = new Vector2(_movementSpeed * moveX * Time.deltaTime, _playerRb.velocity.y);
            UpdateSpriteBasedOnVelocity();
        }

        private void UpdateSpriteBasedOnVelocity()
        {
            // Switch Direction based on Movement
            // Can be moved into a separate script
            if (_playerRb.velocity.x < 0)
            {
                _playerSprite.flipX = true;
            }
            else if (_playerRb.velocity.x > 0)
            {
                _playerSprite.flipX = false;
            }
        }

        #endregion

        #region Jump

        private void HandleJump()
        {
            if (Input.GetButtonDown(ControlConstants.JumpButton) && !_jumped && _playerCollision.IsOnGround)
            {
                _playerRb.drag = 0;
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, _jumpVelocity);

                _jumped = true;

                OnPlayerJumped?.Invoke();
            }

            if (_playerRb.velocity.y < 0)
            {
                if (_playerRb.velocity.y < _linearDragThreasholdVelocity)
                {
                    _playerRb.drag = 0;
                }

                _playerRb.velocity += Time.deltaTime * Physics2D.gravity.y * (_fallMultiplier - 1) * Vector2.up;
            }
            else if (_playerRb.velocity.y > 0)
            {
                if (_playerRb.velocity.y > _linearDragThreasholdVelocity)
                {
                    _playerRb.drag = 0;
                }

                if (!Input.GetButton(ControlConstants.JumpButton))
                {
                    _playerRb.velocity += Time.deltaTime * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Vector2.up;
                }
                else
                {
                    _playerRb.velocity -= Time.deltaTime * _jumpDecelearationRate * Vector2.up;
                }
            }
        }

        #endregion

        #region Dash Movement

        private void HandleDash()
        {
            if (_dashUsed || !Input.GetButtonDown(ControlConstants.DashButton))
            {
                return;
            }

            float xAxisRaw = Input.GetAxisRaw(ControlConstants.HorizontalAxis);
            float yAxisRaw = Input.GetAxisRaw(ControlConstants.VerticalAxis);

            Dash(xAxisRaw, yAxisRaw);
        }

        private void Dash(float xDirection, float yDirection)
        {
            if (xDirection == 0 && yDirection == 0)
            {
                xDirection = _playerSprite.flipX ? -1 : 1;
            }

            _playerRb.velocity = Vector2.zero;
            Vector2 direction = new Vector2(xDirection, yDirection);

            _playerRb.velocity = direction.normalized * _dashSpeed;
            _dashActive = true;
            _dashUsed = true;

            StartCoroutine(DashWait());
        }

        private IEnumerator DashWait()
        {
            yield return new WaitForSeconds(_dashEffectTime);

            _dashActive = false;
            _playerRb.velocity = Vector2.zero;
        }

        #endregion

        private void UpdateMovementVariables()
        {
            if (_playerCollision.IsOnGround)
            {
                _jumped = false;
                _dashUsed = false;
            }
        }

        #endregion
    }
}