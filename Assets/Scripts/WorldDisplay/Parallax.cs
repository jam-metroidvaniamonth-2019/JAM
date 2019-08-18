using UnityEngine;

namespace WorldDisplay
{
    public class Parallax : MonoBehaviour
    {
        [SerializeField] private float _parallaxRate;
        [SerializeField] private Transform _mainCamera;

        private float _length;
        private float _startPosition;


        private void Start()
        {
            _startPosition = transform.position.x;
            _length = GetComponent<SpriteRenderer>().bounds.size.x;
        }

        private void Update()
        {
            float temp = _mainCamera.position.x * (1 - _parallaxRate);
            float distance = _mainCamera.position.x * _parallaxRate;

            transform.position = new Vector3(_startPosition + distance, transform.position.y, transform.position.z);

            if (temp > _startPosition + _length)
            {
                _startPosition += _length;
            }
            else if (temp < _startPosition - _length)
            {
                _startPosition -= _length;
            }
        }
    }
}