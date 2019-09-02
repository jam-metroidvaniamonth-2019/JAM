using Player.Movement;
using UnityEngine;
using Utils;

namespace CustomCamera
{
    public class PlayerCameraFollower : MonoBehaviour
    {
        [Header("Player Follower")]
        [SerializeField] private float _waitAfterJumpCatchUp;
        [SerializeField] private float _playerXLockBuffer = 15;

        private Transform _player;
        private PlayerMovement _playerMovement;
        private Rigidbody2D _playerRb;

        private float _catchUpCountDownTimer;
        private Vector3 _currentTargetPosition;


        #region Unity Functions

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag(TagManager.Player).transform;
            _playerMovement = _player.GetComponent<PlayerMovement>();
            _playerRb = _player.GetComponent<Rigidbody2D>();

            _currentTargetPosition = Vector3.zero;

            _playerMovement.OnPlayerJumped += PlayerJumped;
        }

        private void Update()
        {
            if (_catchUpCountDownTimer <= 0)
            {
                _currentTargetPosition.y = _player.position.y;
            }
            else
            {
                _catchUpCountDownTimer -= Time.deltaTime;
            }

            float leftXPosition = _player.position.x - _playerXLockBuffer / 2.0f;
            float rightXPosition = _player.position.x + _playerXLockBuffer / 2.0f;

            if (_currentTargetPosition.x < leftXPosition)
            {
                _currentTargetPosition.x = leftXPosition;
            }
            else if (_currentTargetPosition.x > rightXPosition)
            {
                _currentTargetPosition.x = rightXPosition;
            }

            transform.position = _currentTargetPosition;
        }

        private void OnDestroy()
        {
            _playerMovement.OnPlayerJumped -= PlayerJumped;
        }

        #endregion

        #region External Functions

        private void PlayerJumped() => _catchUpCountDownTimer = _waitAfterJumpCatchUp;

        #endregion
    }
}