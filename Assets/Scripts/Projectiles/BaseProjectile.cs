using UnityEngine;

namespace Projectiles
{
    public class BaseProjectile : MonoBehaviour
    {
        [SerializeField] private float _launchSpeed = 20;
        [SerializeField] private float _lifetime;

        public float LaunchSpeed => _launchSpeed;

        private float _currentLifeTime;

        #region Unity Event Functions

        private void Start() => _currentLifeTime = _lifetime;

        private void Update()
        {
            _currentLifeTime -= Time.deltaTime;
            if (_currentLifeTime <= 0)
            {
                DestroyProjectile();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            DestroyProjectile();
        }

        #endregion

        #region Helpers

        private void DestroyProjectile()
        {
            // TODO: Add effect to destroy projectile
            // TODO: Add damage to the object if possible
            Destroy(gameObject);
        }

        #endregion
    }
}