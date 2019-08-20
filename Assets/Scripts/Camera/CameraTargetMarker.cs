using Cinemachine;
using UnityEngine;
using Utils;

namespace CustomCamera
{
    public class CameraTargetMarker : MonoBehaviour
    {
        [SerializeField] private float _minActivationDistance;
        [SerializeField] [Range(0, 10)] private float _targetPriority;
        [SerializeField] private float _targetRadius;

        private Transform _player;
        private CinemachineTargetGroup _targetCamera;

        private bool _targetAdded;

        private void Start()
        {
            _player = GameObject.FindGameObjectWithTag(TagManager.Player).transform;
            _targetCamera = GameObject.FindGameObjectWithTag(TagManager.TargetCamera).GetComponent<CinemachineTargetGroup>();
        }

        private void Update()
        {
            if (Vector2.Distance(_player.position, transform.position) <= _minActivationDistance && !_targetAdded)
            {
                _targetAdded = true;
                _targetCamera.AddMember(transform, _targetPriority, _targetRadius);
            }

            else if (Vector2.Distance(_player.position, transform.position) > _minActivationDistance && _targetAdded)
            {
                _targetAdded = false;
                _targetCamera.RemoveMember(transform);
            }
        }
    }
}