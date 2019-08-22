using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Interactibles.Followers
{
    public class FloatyFollowTarget : MonoBehaviour
    {
        [Header("Follow")]
        [SerializeField] private float _minFollowTargetRadius;
        [SerializeField] private float _maxFollowTargetRadius;
        [SerializeField] private Vector3 _followTargetOffset;

        [Header("Target")]
        [SerializeField] private float _targetCLosePointChangeRate;
        [SerializeField] private float _targetLerpSpeed;
        [SerializeField] private Transform _target;

        private float _currentClosePointTimer;
        private Vector3 _currentClosePointOffset;

        #region Unity Functions

        private void Start() => ResetTargetFollowing();

        private void Update()
        {
            _currentClosePointTimer -= Time.deltaTime;
            if (_currentClosePointTimer <= 0)
            {
                ResetTargetFollowing();
            }

            transform.position = Vector3.Lerp(transform.position, _currentClosePointOffset + _target.position, _targetLerpSpeed * Time.deltaTime);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireSphere(_target.position + _currentClosePointOffset, _minFollowTargetRadius);
            Gizmos.DrawWireSphere(_target.position + _currentClosePointOffset, _maxFollowTargetRadius);
        }

        #endregion

        #region Utility Functions

        private void ResetTargetFollowing()
        {
            _currentClosePointTimer = _targetCLosePointChangeRate;
            _currentClosePointOffset =
                _followTargetOffset * Random.Range(_minFollowTargetRadius, _maxFollowTargetRadius);
        }

        #endregion
    }
}