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

        [Header("Floaty")]
        [SerializeField] private float _amplitude;
        [SerializeField] private float _frequency;

        private float _currentClosePointTimer;
        private Vector3 _currentClosePointOffset;

        private Vector3 _lerpPosition;
        private Vector3 _finalPosition;


        #region Unity Functions

        private void Start() => ResetTargetFollowing();

        private void Update()
        {
            _currentClosePointTimer -= Time.deltaTime;
            if (_currentClosePointTimer <= 0)
            {
                ResetTargetFollowing();
            }

            _lerpPosition = Vector3.Lerp(_lerpPosition, _currentClosePointOffset + _target.position, _targetLerpSpeed * Time.deltaTime);

            float yOffset = Mathf.Sin(Time.time * _frequency) * _amplitude;
            _finalPosition = _lerpPosition;
            _finalPosition.y += yOffset;

            transform.position = _finalPosition;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireSphere(_target.position + _currentClosePointOffset, _minFollowTargetRadius);
            Gizmos.DrawWireSphere(_target.position + _currentClosePointOffset, _maxFollowTargetRadius);
        }

        #endregion

        #region External Functions

        public void UpdateTarget(Transform target) => _target = target;

        public bool TargetReachedPosition() => Vector3.Distance(transform.position, _target.position) <= _maxFollowTargetRadius;

        #endregion

        #region Utility Functions

        private void ResetTargetFollowing()
        {
            _currentClosePointTimer = _targetCLosePointChangeRate;
            _currentClosePointOffset =
                _followTargetOffset * Random.Range(_minFollowTargetRadius, _maxFollowTargetRadius);

            _lerpPosition = transform.position;
        }

        #endregion
    }
}