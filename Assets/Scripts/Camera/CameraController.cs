using System.Collections.Generic;
using UnityEngine;

namespace CustomCamera
{
    public class CameraController : MonoBehaviour
    {
        [Header("Generic Controls")]
        [SerializeField] private Vector3 _playerFollowOffset;
        [SerializeField] private float _lerpSpeed;
        [SerializeField] private List<Transform> _targets;

        #region Unity Functions

        private void Update()
        {
            Vector3 lookAtPosition = GetLookAtPointPoint();
            lookAtPosition += _playerFollowOffset;

            transform.position = Vector3.Lerp(transform.position, lookAtPosition, _lerpSpeed * Time.deltaTime);
        }

        #endregion

        #region External Functions

        public int AddTargetToCamera(Transform target)
        {
            _targets.Add(target);
            return _targets.Count - 1;
        }

        public void RemoveTargetFromCamera(int index) => _targets.RemoveAt(index);

        public void UpdatePlayerOffset(Vector3 offsetPosition) => _playerFollowOffset = offsetPosition;

        #endregion

        #region Utility Functions

        private Vector3 GetLookAtPointPoint()
        {
            float xPosition = 0;
            float yPosition = 0;

            for (int i = 0; i < _targets.Count; i++)
            {
                xPosition += _targets[i].position.x;
                yPosition += _targets[i].position.y;
            }

            xPosition /= _targets.Count;
            yPosition /= _targets.Count;

            return new Vector3(xPosition, yPosition, 0);
        }

        #endregion

        #region Singleton

        public static CameraController instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        #endregion
    }
}