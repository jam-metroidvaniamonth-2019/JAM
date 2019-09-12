using System.Collections.Generic;
using UnityEngine;

namespace WorldDisplay
{
    public class ParallaxV2 : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _parallaxRate;
        [SerializeField] private float _chokeAmount;
        [SerializeField] private List<SpriteRenderer> _children;

        private float _cameraWidth;

        private Vector3 _lastScreenPosition;

        #region Unity Functions

        private void Start()
        {
            _cameraWidth = _mainCamera.orthographicSize * Screen.width / Screen.height;
            _lastScreenPosition = _mainCamera.transform.position;
        }

        private void LateUpdate()
        {
            RePositionChildren();

            float difference = _mainCamera.transform.position.x - _lastScreenPosition.x;
            transform.Translate(_parallaxRate * difference * Vector3.right);

            _lastScreenPosition = _mainCamera.transform.position;
        }

        #endregion

        #region Parallax Effect

        private void RePositionChildren()
        {
            if (_children.Count > 2)
            {
                SpriteRenderer firstChildRenderer = _children[0];
                SpriteRenderer lastChildRenderer = _children[_children.Count - 1];

                Transform firstChild = _children[0].transform;
                Transform lastChild = _children[_children.Count - 1].transform;

                float firstChildHalfWidth = _children[0].bounds.extents.x;
                float lastChildHalfWidth = _children[_children.Count - 1].bounds.extents.x;

                if (_mainCamera.transform.position.x + _cameraWidth > lastChild.position.x + lastChildHalfWidth)
                {
                    firstChild.SetAsLastSibling();
                    firstChild.position = new Vector3(lastChild.position.x + firstChildHalfWidth * 2,
                        firstChild.position.y, firstChild.position.z);

                    _children.RemoveAt(0);
                    _children.Add(firstChildRenderer);
                }
                else if (_mainCamera.transform.position.x - _cameraWidth < firstChild.position.x - firstChildHalfWidth)
                {
                    lastChild.SetAsFirstSibling();
                    lastChild.position = new Vector3(firstChild.position.x - lastChildHalfWidth * 2,
                        lastChild.position.y, lastChild.position.z);

                    _children.RemoveAt(_children.Count - 1);
                    _children.Insert(0, lastChildRenderer);
                }
            }
        }

        #endregion
    }
}