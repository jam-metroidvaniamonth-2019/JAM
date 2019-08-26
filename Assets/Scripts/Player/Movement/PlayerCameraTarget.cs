using System;
using Cinemachine;
using UnityEngine;
using Utils;

namespace Player.Movement
{
    public class PlayerCameraTarget : MonoBehaviour
    {
        [Header("Target")]
        [SerializeField] private float _minYDistance;
        [SerializeField] private float _targetStopMinYDistance;
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private Vector3 _playerFollowOffset;

        [Header("Player")]
        [SerializeField] private Transform _player;
        [SerializeField] private float _playerWeight;
        [SerializeField] private CinemachineTargetGroup _mainTargetGroup;

        private Vector3 _playerPosition;
        private Vector3 _targetPosition;

        private bool _targetingStarted;

        private Transform _targetYModifier;

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
                if (!_targetingStarted)
                {
                    _mainTargetGroup.AddMember(_player, _playerWeight, 0);
                }

                _targetingStarted = true;
            }
            else if (Mathf.Abs(_targetPosition.y - _playerPosition.y) < _targetStopMinYDistance)
            {
                if (_targetingStarted)
                {
                    _mainTargetGroup.RemoveMember(_player);
                }

                _targetingStarted = false;
            }

            if (_targetYModifier)
            {
                _targetPosition.y = Mathf.Lerp(_targetPosition.y, _targetYModifier.position.y,
                    _lerpSpeed * Time.deltaTime);
            }
            else if (_targetingStarted)
            {
                _targetPosition.y = Mathf.Lerp(_targetPosition.y, _playerPosition.y, _lerpSpeed * Time.deltaTime);
            }


            transform.position = _targetPosition + _playerFollowOffset;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(TagManager.TargetFollowerModifier))
            {
                return;
            }

            _targetYModifier = other.gameObject.transform;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag(TagManager.TargetFollowerModifier))
            {
                return;
            }

            _targetYModifier = null;
        }

        #endregion
    }
}