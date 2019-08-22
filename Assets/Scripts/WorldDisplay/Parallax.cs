using UnityEngine;

namespace WorldDisplay
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private float _parallaxRate;
        [SerializeField] private Transform _mainCamera;
        [SerializeField] private float _maxDistance;

        private float _length;
        private float _startPosition;


        private void Start()
        {
            _startPosition = transform.position.x;
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Update()
        {
            float distance = _mainCamera.position.x * _parallaxRate;
            if (Mathf.Abs(distance) > _maxDistance)
            {
                return;
            }

            transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);
        }
    }
}