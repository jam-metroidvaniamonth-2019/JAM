using System;
using System.Collections;
using Player.General;
using UnityEngine;
using Utils;

namespace Player.Movement
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Systems Affector")]
        [SerializeField] private PlayerController _playercontroller;

        [Header("Movement")]
        [SerializeField] private float _movementSpeed = 250;
        [SerializeField] private float _linearDrag = 50;
        [SerializeField] private float _linearDragThresholdVelocity = 0.5f;
        [SerializeField] private SpriteRenderer _playerSprite;
        [SerializeField] private SpriteRenderer _playerBagSprite;
        [SerializeField] private float _movementTolerance = 0.3f;

        [Header("Jump")]
        [SerializeField] private float _jumpVelocity = 7;
        [SerializeField] private float _fallMultiplier = 5f;
        [SerializeField] private float _lowJumpMultiplier = 2;
        [SerializeField] private float _jumpDecelearationRate = 10;

        [Header("Dash")]
        [SerializeField] private float _dashSpeed = 500f;
        [SerializeField] private float _dashEffectTime = 0.3f;
        [SerializeField] private ParticleSystem _playerDashEffect;

        public delegate void PlayerDashed();
        public PlayerDashed OnPlayerDashed;

        private Rigidbody2D _playerRb;
        private PlayerCollision _playerCollision;

        // Dash
        private bool _dashActive;
        private bool _dashUsed;

        // Jump
        private bool _jumped;

        // Movement Controls
        private bool _movementEnabled;

        // Control Buttons
        private float _moveX;
        private float _moveXRaw;
        private float _moveYRaw;
        private bool _jumpButtonPressed;
        private bool _jumpButtonJustPressed;
        private bool _dashButtonPressed;

        // Delegates
        public delegate void PlayerJumped();
        public PlayerJumped OnPlayerJumped;

        #region Unity Functions

        private void Awake() => _movementEnabled = true;

        private void Start()
        {
            _playerRb = GetComponent<Rigidbody2D>();
            _playerCollision = GetComponent<PlayerCollision>();
        }

        private void Update()
        {
            _moveX = Input.GetAxis(ControlConstants.HorizontalAxis);
            _moveXRaw = Input.GetAxisRaw(ControlConstants.HorizontalAxis);

            _moveYRaw = Input.GetAxisRaw(ControlConstants.VerticalAxis);

            _jumpButtonPressed = Input.GetButton(ControlConstants.JumpButton);
            _jumpButtonJustPressed = Input.GetButtonDown(ControlConstants.JumpButton);

            _dashButtonPressed = Input.GetButtonDown(ControlConstants.DashButton);
        }

        private void FixedUpdate()
        {
            UpdateMovementVariables();

            if (_movementEnabled)
            {
                HandleDash();

                if (!_dashActive)
                {
                    HandleHorizontalMovement();
                    HandleJump();
                }
            }
        }

        #endregion

        #region Movement

        #region Horizontal Movement

        private void HandleHorizontalMovement()
        {
            if (Math.Abs(_moveXRaw) < _movementTolerance && _playerCollision.IsOnGround)
            {
                _playerRb.drag = _linearDrag;
            }
            else
            {
                _playerRb.drag = 0;
            }

            _playerRb.velocity = new Vector2(_movementSpeed * _moveX * Time.fixedDeltaTime, _playerRb.velocity.y);
            UpdateSpriteBasedOnVelocity();
        }

        private void UpdateSpriteBasedOnVelocity()
        {
            // Switch Direction based on Movement
            // Can be moved into a separate script
            if (_playerRb.velocity.x < 0)
            {
                _playerSprite.flipX = true;
                _playerBagSprite.flipX = true;
            }
            else if (_playerRb.velocity.x > 0)
            {
                _playerSprite.flipX = false;
                _playerBagSprite.flipX = false;
            }
        }

        #endregion

        #region Jump

        private void HandleJump()
        {
            if (_jumpButtonJustPressed && !_jumped && _playerCollision.IsOnGround)
            {
                _playerRb.drag = 0;
                _playerRb.velocity = new Vector2(_playerRb.velocity.x, _jumpVelocity);

                _jumped = true;

                OnPlayerJumped?.Invoke();
            }

            if (_playerRb.velocity.y < 0)
            {
                if (_playerRb.velocity.y < _linearDragThresholdVelocity)
                {
                    _playerRb.drag = 0;
                }

                _playerRb.velocity += Time.fixedDeltaTime * Physics2D.gravity.y * (_fallMultiplier - 1) * Vector2.up;
            }
            else if (_playerRb.velocity.y > 0)
            {
                if (_playerRb.velocity.y > _linearDragThresholdVelocity)
                {
                    _playerRb.drag = 0;
                }

                if (!_jumpButtonPressed)
                {
                    _playerRb.velocity += Time.fixedDeltaTime * Physics2D.gravity.y * (_lowJumpMultiplier - 1) * Vector2.up;
                }
                else
                {
                    _playerRb.velocity -= Time.fixedDeltaTime * _jumpDecelearationRate * Vector2.up;
                }
            }
        }

        #endregion

        #region Dash Movement

        private void HandleDash()
        {
            if (_dashUsed || !_dashButtonPressed || !_playercontroller.PlayerHasDash)
            {
                return;
            }

            Dash(_moveXRaw, _moveYRaw);
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

            _playerDashEffect.Play();
            ParticleSystemRenderer renderer = _playerDashEffect.GetComponent<ParticleSystemRenderer>();
            renderer.flip = _playerSprite.flipX ? new Vector3(1, 0, 0) : new Vector3(-1, 0, 0);

            OnPlayerDashed?.Invoke();

            StartCoroutine(DashWait());
        }

        private IEnumerator DashWait()
        {
            yield return new WaitForSeconds(_dashEffectTime);

            _dashActive = false;
            _playerRb.velocity = Vector2.zero;

            _playerDashEffect.Stop();
        }

        #endregion

        private void UpdateMovementVariables()
        {
            if (_playerCollision.IsOnGround && _playerRb.velocity.y == 0)
            {
                _jumped = false;
                _dashUsed = false;
            }
        }

        #endregion

        #region External Functions

        public void EnableMovement() => _movementEnabled = true;

        public void DisableMovement()
        {
            _movementEnabled = false;
            _playerRb.drag = _linearDrag;
            _playerRb.velocity = Vector3.zero;
        }

        #endregion
    }
}