using UnityEngine;

namespace Player.Movement
{
    public class PlayerCollision : MonoBehaviour
    {
        [Header("Layers")]
        [SerializeField] private LayerMask _groundMask;

        [Header("Collision Data")]
        [SerializeField] private float _collisionCheckRadius;
        [SerializeField] private Vector2 _bottomOffset;

        [Header("Display Only Values")]
        [SerializeField] private bool _isOnGround;

        public bool IsOnGround => _isOnGround;

        public delegate void GroundStatusChanged(bool groundStatus);
        public GroundStatusChanged OnGroundStatusChanged;

        #region Unity Function

        private void Update()
        {
            bool isOnGround = Physics2D.OverlapCircle((Vector2)transform.position + _bottomOffset, _collisionCheckRadius, _groundMask);
            EmitAndUpdateGroundStatusOnChange(isOnGround);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere((Vector2)transform.position + _bottomOffset, _collisionCheckRadius);
        }

        #endregion

        #region Event Functions

        private void EmitAndUpdateGroundStatusOnChange(bool isOnGround)
        {
            if (isOnGround != _isOnGround)
            {
                OnGroundStatusChanged?.Invoke(isOnGround);
            }

            _isOnGround = isOnGround;
        }

        #endregion
    }
}