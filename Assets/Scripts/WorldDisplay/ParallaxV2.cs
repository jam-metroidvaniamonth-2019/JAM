using System.Collections.Generic;
using UnityEngine;

namespace WorldDisplay
{
    public class ParallaxV2 : MonoBehaviour
    {
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private float _parallaxRate;
        [SerializeField] private float _chokeAmount;

        private Vector2 _screenBounds;
        private List<Transform> _children;
        private float _halfObjectWidth;

        private Vector3 _lastScreenPosition;

        #region Unity Functions

        private void Start()
        {
            _screenBounds = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, _mainCamera.transform.position.z));

            SpriteRenderer[] childRenderers = gameObject.GetComponentsInChildren<SpriteRenderer>();
            _children = new List<Transform>();
            foreach (SpriteRenderer spriteRenderer in childRenderers)
            {
                _children.Add(spriteRenderer.transform);
            }


            _halfObjectWidth = _children[1].GetComponent<SpriteRenderer>().bounds.extents.x;
            _halfObjectWidth -= _chokeAmount;

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
                Transform firstChild = _children[0];
                Transform lastChild = _children[_children.Count - 1];

                if (_mainCamera.transform.position.x + _screenBounds.x > lastChild.position.x + _halfObjectWidth)
                {
                    firstChild.SetAsLastSibling();
                    firstChild.position = new Vector3(lastChild.position.x + _halfObjectWidth * 2, lastChild.position.y, lastChild.position.z);

                    _children.RemoveAt(0);
                    _children.Add(firstChild);

                }
                else if (_mainCamera.transform.position.x - _screenBounds.x < firstChild.position.x - _halfObjectWidth)
                {
                    lastChild.SetAsFirstSibling();
                    lastChild.position = new Vector3(firstChild.position.x - _halfObjectWidth * 2, firstChild.position.y, firstChild.position.z);

                    _children.RemoveAt(_children.Count - 1);
                    _children.Insert(0, lastChild);
                }

            }
        }

        #endregion
    }
}