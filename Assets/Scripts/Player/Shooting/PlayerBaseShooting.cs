using UnityEngine;
using Utils;

namespace Player.Shooting
{
    public class PlayerBaseShooting : MonoBehaviour
    {
        [Header("Display Objects")]
        [SerializeField] private GameObject _shootDirectionDisplayGameObject;
        [SerializeField] private Transform _playerTransform;

        [Header("Extra Controls")]
        [SerializeField] private float _rotationOffset = 90;
        [SerializeField] private float _inactiveHideTimer = 1;

        // Display State Objects
        private Transform _shootDirectionDisplay;
        private SpriteRenderer _shootDirectionDisplayRenderer;

        // Timer
        private float _currentInactiveTimer;

        #region Unity Event Functions

        private void Start()
        {
            _shootDirectionDisplay = _shootDirectionDisplayGameObject.transform;
            _shootDirectionDisplayRenderer = _shootDirectionDisplayGameObject.GetComponent<SpriteRenderer>();
        }

        private void Update()
        {
            float xMovement = Input.GetAxis(ControlConstants.HorizontalShootAxis);
            float yMovement = Input.GetAxis(ControlConstants.VerticalShootAxis);

            bool isMouseMoving = Input.GetAxis(ControlConstants.MouseX) != 0 || Input.GetAxis(ControlConstants.MouseY) != 0;

            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            // Basically means that don't update anything if nothing is touched
            if (xMovement == 0 && yMovement == 0 && !isMouseMoving)
            {
                if (_currentInactiveTimer > 0)
                {
                    _currentInactiveTimer -= Time.deltaTime;
                }
                else
                {
                    _shootDirectionDisplayRenderer.enabled = false;
                }
            }
            else
            {
                _shootDirectionDisplayRenderer.enabled = true;
                _currentInactiveTimer = _inactiveHideTimer;

                float rotationAngle = Mathf.Atan2(yMovement, xMovement) * Mathf.Rad2Deg;
                if (isMouseMoving)
                {
                    rotationAngle = Mathf.Atan2(worldPoint.y - _playerTransform.position.y,
                                        worldPoint.x - _playerTransform.position.x) * Mathf.Rad2Deg;
                }

                rotationAngle -= _rotationOffset;

                _shootDirectionDisplay.rotation = Quaternion.Euler(0, 0, rotationAngle);
            }
        }

        #endregion
    }
}