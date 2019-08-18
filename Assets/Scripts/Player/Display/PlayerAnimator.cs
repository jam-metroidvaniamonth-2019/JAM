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

        #endregion
    }
}