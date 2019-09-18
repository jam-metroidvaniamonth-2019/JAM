using Player.General;
using Player.Movement;
using Projectiles;
using UnityEngine;
using Utils;

namespace Player.Shooting
{
    public class PlayerShooter : MonoBehaviour
    {
        [Header("Display Objects")]
        [SerializeField] private GameObject _playerBowDisplay;
        [SerializeField] private GameObject _playerSlingShotDisplay;
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private GameObject _weaponDisplayEffectPrefab;
        [SerializeField] private PlayerController _playerController;

        [Header("Extra Controls")]
        [SerializeField] private float _rotationOffset = 90;
        [SerializeField] private float _timeSlowActiveWait = 0.2f;
        [SerializeField] private float _autoShootWait = 3f;
        [SerializeField] private float _timeSlowAmount = 0.3f;

        [Header("Bullet")]
        [SerializeField] private float _shotWaitTime = 0.5f;
        [SerializeField] private GameObject _arrowPrefab;
        [SerializeField] private GameObject _playerAntidoteArrowPrefab;
        [SerializeField] private GameObject _stonePrefab;
        [SerializeField] private Transform _bulletHolder;
        [SerializeField] private Transform _bowShootingPoint;
        [SerializeField] private Transform _slingShotShootingPoint;

        [Header("Shooting Angle Limits")]
        [SerializeField] [Range(10, 40)] private float _bowAngleRestriction;
        [SerializeField] [Range(10, 40)] private float _slingShotAngleRestriction;
        [SerializeField] private SpriteRenderer _playerSprite;
        [SerializeField] private SpriteRenderer _playerBagSprite;
        [SerializeField] private PlayerMovement _playerMovement;

        public delegate void PlayerShot(bool playerHasBow);
        public PlayerShot OnPlayerShot;

        public delegate void PlayerShootInPosition(bool playerHasBow);
        public PlayerShootInPosition OnPlayerShootInPosition;

        // Display State Objects
        private Transform _shootBowDirectionDisplay;
        private SpriteRenderer _shootBowDirectionDisplayRenderer;
        private Transform _shootSlingShotDirectionDisplay;
        private SpriteRenderer _shootSlingShotDirectionDisplayRenderer;

        // Shooting Status Controls
        private float _timeBeforeLastShot;
        private float _timeSlowTimer;
        private bool _timeSlowFired;
        private float _autoShootTimer;
        private bool _triggerHeldDown;
        private float _directionLockedAngle;

        // These are required as the Right Trigger is detected
        // as an axis and we need to convert it into a button
        private float _currentRightTriggerState;
        private bool _rightTriggerStateChanged;

        public delegate void TimeSlowActive();
        public TimeSlowActive OnTimeSlowActive;

        private bool _disableShooting;

        #region Unity Functions

        private void Start()
        {
            _shootBowDirectionDisplay = _playerBowDisplay.transform;
            _shootBowDirectionDisplayRenderer = _playerBowDisplay.GetComponent<SpriteRenderer>();

            _shootSlingShotDirectionDisplay = _playerSlingShotDisplay.transform;
            _shootSlingShotDirectionDisplayRenderer = _playerSlingShotDisplay.GetComponent<SpriteRenderer>();

            _triggerHeldDown = false;
        }

        private void Update()
        {
            // Lock Direction based on Sprite Flip Methods
            if (_playerSprite.gameObject.activeInHierarchy)
            {
                _directionLockedAngle = _playerSprite.flipX ? 180 : 0;
            }
            else
            {
                _directionLockedAngle = _playerBagSprite.flipX ? 180 : 0;
            }


            _timeBeforeLastShot += Time.deltaTime;

            UpdateRightTriggerState();

            if ((_currentRightTriggerState == -1 && _rightTriggerStateChanged) || Input.GetMouseButtonDown(0))
            {
                SetupBulletShooting();
            }

            if (((_currentRightTriggerState == 0 && _rightTriggerStateChanged) || Input.GetMouseButtonUp(0)) &&
                _triggerHeldDown)
            {
                ShootBullet();
            }

            // Update Timers and Fire Events
            if (_triggerHeldDown)
            {
                _timeSlowTimer -= Time.deltaTime;
                _autoShootTimer -= Time.deltaTime / Time.timeScale;

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

        #region External Functions

        public void DisableShooting() => _disableShooting = true;

        public void EnableShooting() => _disableShooting = false;

        #endregion

        #region Shooting Controls

        private void UpdateRotation()
        {
            if (!_triggerHeldDown)
            {
                _shootBowDirectionDisplayRenderer.enabled = false;
                _shootSlingShotDirectionDisplayRenderer.enabled = false;
                return;
            }

            if (_playerController.PlayerHasBow)
            {
                _shootBowDirectionDisplayRenderer.enabled = true;
            }
            else
            {
                _shootSlingShotDirectionDisplayRenderer.enabled = true;
            }

            float xMovement = Input.GetAxis(ControlConstants.HorizontalShootAxis);
            float yMovement = Input.GetAxis(ControlConstants.VerticalShootAxis);

            bool isMouseMoving = Input.GetAxis(ControlConstants.MouseX) != 0 ||
                                 Input.GetAxis(ControlConstants.MouseY) != 0;

            Vector3 worldPoint = _mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,
                Input.mousePosition.y, _mainCamera.nearClipPlane));

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
            // This is a hack to lock angles going from 
            // -180 to 180
            if (rotationAngle < -90)
            {
                rotationAngle = ExtensionFunctions.To360Angle(rotationAngle);
            }

            if (_playerController.PlayerHasBow)
            {
                rotationAngle = Mathf.Clamp(rotationAngle, _directionLockedAngle - _bowAngleRestriction,
                    _directionLockedAngle + _bowAngleRestriction);
            }
            else
            {
                rotationAngle = Mathf.Clamp(rotationAngle, _directionLockedAngle - _slingShotAngleRestriction,
                    _directionLockedAngle + _slingShotAngleRestriction);
            }

            _shootBowDirectionDisplay.rotation = Quaternion.Euler(0, 0, rotationAngle);
            _shootSlingShotDirectionDisplay.rotation = Quaternion.Euler(0, 0, rotationAngle);
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
            if (_disableShooting)
            {
                return;
            }

            _triggerHeldDown = true;
            _timeSlowTimer = _timeSlowActiveWait;
            _autoShootTimer = _autoShootWait;

            _shootBowDirectionDisplay.rotation = Quaternion.Euler(0, 0, _directionLockedAngle);
            _shootSlingShotDirectionDisplay.rotation = Quaternion.Euler(0, 0, _directionLockedAngle);

            // Instantiate(_weaponDisplayEffectPrefab, _shootBowDirectionDisplay.position, Quaternion.identity);

            _playerMovement.DisableMovement();

            OnPlayerShootInPosition?.Invoke(_playerController.PlayerHasBow);
        }

        private void ShootBullet()
        {
            _triggerHeldDown = false;
            _timeSlowFired = false;
            Time.timeScale = 1;

            _playerMovement.EnableMovement();
            OnPlayerShot?.Invoke(_playerController.PlayerHasBow);

            if (_timeBeforeLastShot < _shotWaitTime)
            {
                return;
            }

            _timeBeforeLastShot = 0;

            Vector3 shootingPosition = _playerController.PlayerHasBow
                ? _bowShootingPoint.position
                : _slingShotShootingPoint.position;

            GameObject bulletPrefab = _playerController.PlayerHasBow ? _playerController.PlayerHasAntidote ? _playerAntidoteArrowPrefab : _arrowPrefab : _stonePrefab;
            GameObject bulletInstance = Instantiate(bulletPrefab, shootingPosition, Quaternion.identity);

            float launchSpeed = bulletInstance.GetComponent<BaseProjectile>().LaunchSpeed;
            float xVelocity = Mathf.Cos(_shootBowDirectionDisplay.rotation.eulerAngles.z * Mathf.Deg2Rad);
            float yVelocity = Mathf.Sin(_shootBowDirectionDisplay.rotation.eulerAngles.z * Mathf.Deg2Rad);
            Vector2 launchDirection = new Vector2(xVelocity, yVelocity);

            bulletInstance.transform.rotation = _shootBowDirectionDisplay.rotation;

            bulletInstance.GetComponent<Rigidbody2D>().velocity = launchSpeed * launchDirection.normalized;
            bulletInstance.transform.SetParent(_bulletHolder);
        }

        #endregion

        #region Trigger To Button Conversion

        // Only Handle Right Trigger
        private void UpdateRightTriggerState()
        {
            _rightTriggerStateChanged = false;
            float triggerActiveState = Input.GetAxis(ControlConstants.Shoot);

            if (triggerActiveState != 0 && triggerActiveState != -1)
            {
                return;
            }

            if (_currentRightTriggerState != triggerActiveState)
            {
                _currentRightTriggerState = triggerActiveState;
                _rightTriggerStateChanged = true;
            }
        }

        #endregion
    }
}