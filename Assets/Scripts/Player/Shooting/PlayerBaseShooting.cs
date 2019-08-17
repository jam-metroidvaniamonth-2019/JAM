﻿using Projectiles;
using UnityEngine;
using Utils;

namespace Player.Shooting
{
    public class PlayerBaseShooting : MonoBehaviour
    {
        [Header("Display Objects")]
        [SerializeField] private GameObject _shootDirectionDisplayGameObject;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Camera _mainCamera;

        [Header("Extra Controls")]
        [SerializeField] private float _rotationOffset = 90;
        [SerializeField] private float _timeSlowActiveWait = 0.2f;
        [SerializeField] private float _autoShootWait = 3f;
        [SerializeField] private float _timeSlowAmount = 0.3f;

        [Header("Bullet")]
        [SerializeField] private GameObject _bulletObject;
        [SerializeField] private Transform _bulletHolder;
        [SerializeField] private Transform _shootingPoint;

        // Display State Objects
        private Transform _shootDirectionDisplay;
        private SpriteRenderer _shootDirectionDisplayRenderer;

        // Controls
        private float _timeSlowTimer;
        private bool _timeSlowFired;
        private float _autoShootTimer;
        private bool _triggerHeldDown;

        public delegate void TimeSlowActive();
        public TimeSlowActive OnTimeSlowActive;

        #region Unity Event Functions

        private void Start()
        {
            _shootDirectionDisplay = _shootDirectionDisplayGameObject.transform;
            _shootDirectionDisplayRenderer = _shootDirectionDisplayGameObject.GetComponent<SpriteRenderer>();

            _triggerHeldDown = false;
        }

        private void Update()
        {
            if (Input.GetButtonDown(ControlConstants.Shoot) || Input.GetMouseButtonDown(0))
            {
                SetupBulletShooting();
            }

            if ((Input.GetButtonUp(ControlConstants.Shoot) || Input.GetMouseButtonUp(0)) && _triggerHeldDown)
            {
                ShootBullet();
            }

            // Update Timers and Fire Events
            if (_triggerHeldDown)
            {

                _timeSlowTimer -= Time.deltaTime;
                _autoShootTimer -= Time.deltaTime;

                if (_timeSlowTimer <= 0)
                {
                    ActivateShootingTimeSlow();
                }

                if (_autoShootTimer <= 0)
                {
                    ShootBullet();
                }
            }

            UpdateRotation();
        }

        #endregion

        #region Shooting Controls

        private void UpdateRotation()
        {
            if (!_triggerHeldDown)
            {
                _shootDirectionDisplayRenderer.enabled = false;
                return;
            }

            _shootDirectionDisplayRenderer.enabled = true;

            float xMovement = Input.GetAxis(ControlConstants.HorizontalShootAxis);
            float yMovement = Input.GetAxis(ControlConstants.VerticalShootAxis);

            bool isMouseMoving = Input.GetAxis(ControlConstants.MouseX) != 0 || Input.GetAxis(ControlConstants.MouseY) != 0;

            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(Input.mousePosition);

            if (xMovement == 0 && yMovement == 0 && !isMouseMoving)
            {
                return;
            }

            float rotationAngle = Mathf.Atan2(yMovement, xMovement) * Mathf.Rad2Deg;
            if (isMouseMoving)
            {
                rotationAngle = Mathf.Atan2(worldPoint.y - _playerTransform.position.y,
                                    worldPoint.x - _playerTransform.position.x) * Mathf.Rad2Deg;
            }

            rotationAngle -= _rotationOffset;

            _shootDirectionDisplay.rotation = Quaternion.Euler(0, 0, rotationAngle);
        }

        private void ActivateShootingTimeSlow()
        {
            if (_timeSlowFired)
            {
                return;
            }

            OnTimeSlowActive?.Invoke();

            _timeSlowFired = true;
            Time.timeScale = _timeSlowAmount;
        }

        private void SetupBulletShooting()
        {
            _triggerHeldDown = true;
            _timeSlowTimer = _timeSlowActiveWait;
            _autoShootTimer = _autoShootWait;
        }

        private void ShootBullet()
        {
            _triggerHeldDown = false;
            _timeSlowFired = false;

            GameObject bulletInstance = Instantiate(_bulletObject, _shootingPoint.position, Quaternion.identity);

            float launchSpeed = bulletInstance.GetComponent<BaseProjectile>().LaunchSpeed;
            float xVelocity = -Mathf.Sin(_shootDirectionDisplay.rotation.eulerAngles.z * Mathf.Deg2Rad);
            float yVelocity = Mathf.Cos(_shootDirectionDisplay.rotation.eulerAngles.z * Mathf.Deg2Rad);
            Vector2 launchDirection = new Vector2(xVelocity, yVelocity);

            bulletInstance.GetComponent<Rigidbody2D>().velocity = launchSpeed * launchDirection.normalized;
            bulletInstance.transform.SetParent(_bulletHolder);

            Time.timeScale = 1;
        }

        #endregion
    }
}