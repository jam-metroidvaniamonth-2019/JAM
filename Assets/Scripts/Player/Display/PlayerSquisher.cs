using Player.Movement;
using UnityEngine;

namespace Player.Display
{
    public class PlayerSquisher : MonoBehaviour
    {
        [Header("Collision Detector")]
        [SerializeField] private PlayerCollision _playerCollision;
        [SerializeField] private PlayerMovement _playerMovement;

        [Header("Squish and Stretch")]
        [SerializeField] private Vector3 _stretchScale;
        [SerializeField] private Vector3 _squashScale;
        [SerializeField] private Vector3 _defaultSpriteScale;
        [SerializeField] private float _morphIntoRate;
        [SerializeField] private float _morphOutRate;

        private bool _isMorphCompleted;
        private bool _isMorphInto;
        private float _lerpAmount;
        private Vector3 _introMorphTarget;

        #region Unity Functions

        private void Start()
        {
            _playerCollision.OnGroundStatusChanged += HandlePlayerCollisionChange;
            _playerMovement.OnPlayerJumped += HandlePlayerJumped;
        }

        private void Update()
        {
            if (_isMorphCompleted)
            {
                return;
            }

            if (_isMorphInto)
            {
                _lerpAmount += Time.deltaTime * _morphIntoRate;
                transform.localScale = Vector3.Lerp(transform.localScale, _introMorphTarget, Time.deltaTime * _morphIntoRate);

                if (_lerpAmount >= 0.99)
                {
                    _isMorphInto = false;
                    _lerpAmount = 0;
                }
            }
            else
            {
                _lerpAmount += Time.deltaTime * _morphOutRate;
                transform.localScale = Vector3.Lerp(transform.localScale, _defaultSpriteScale, Time.deltaTime * _morphOutRate);

                if (_lerpAmount >= 0.99)
                {
                    _isMorphCompleted = true;
                    transform.localScale = _defaultSpriteScale;
                }
            }
        }

        #endregion

        #region Utility Functions

        private void HandlePlayerCollisionChange(bool isOnGround)
        {
            if (isOnGround)
            {
                _introMorphTarget = new Vector3(_squashScale.x * _defaultSpriteScale.x, _squashScale.y * _defaultSpriteScale.y, _squashScale.z * _defaultSpriteScale.z);
                _isMorphInto = true;
                _isMorphCompleted = false;
                _lerpAmount = 0;
            }
        }

        private void HandlePlayerJumped()
        {
            _introMorphTarget = new Vector3(_stretchScale.x * _defaultSpriteScale.x, _stretchScale.y * _defaultSpriteScale.y, _stretchScale.z * _defaultSpriteScale.z);
            _isMorphInto = true;
            _isMorphCompleted = false;
            _lerpAmount = 0;
        }

        #endregion
    }
}