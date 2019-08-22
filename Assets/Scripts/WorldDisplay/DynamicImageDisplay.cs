using UnityEngine;

namespace WorldDisplay
{
    public class DynamicImageDisplay : MonoBehaviour
    {
        [Header("Display Constraints")]
        [SerializeField] private Vector2 _spriteMaskWorldPosition;

        private Transform _spriteMask;

        private void Start() => _spriteMask = transform.GetChild(0);

        private void LateUpdate() => _spriteMask.transform.position = _spriteMaskWorldPosition;
    }
}