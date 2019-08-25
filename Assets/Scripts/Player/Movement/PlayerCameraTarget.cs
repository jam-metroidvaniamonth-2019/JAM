using UnityEngine;
using Utils;

namespace Player.Movement
{
    public class PlayerCameraTarget : MonoBehaviour
    {
        [SerializeField] private float _minYDistance;
        [SerializeField] private float _targetStopMinYDistance;
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private Vector3 _playerFollowOffset;

        private Transform _player;

        private Vector3 _playerPosition;
        private Vector3 _targetPosition;

        private bool _targetingStarted;

        #region Unity Functions

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag(TagManager.Player).transform;
            _targetPosition = _player.position;
        }

        private void LateUpdate()
        {
            _playerPosition = _player.position;

            // Force Snap X Position
            _targetPosition.x = _playerPosition.x;

            // Handle Y Position
            if (Mathf.Abs(_targetPosition.y - _playerPosition.y) > _minYDistance)
            {
                _targetingStarted = true;
            }
            else if (Mathf.Abs(_targetPosition.y - _playerPosition.y) < _targetStopMinYDistance)
            {
                _targetingStarted = false;
            }

            if (_targetingStarted)
            {
                _targetPosition.y = Mathf.Lerp(_targetPosition.y, _playerPosition.y, _lerpSpeed * Time.deltaTime);
            }


            transform.position = _targetPosition + _playerFollowOffset;
        }

        #endregion
    }
}