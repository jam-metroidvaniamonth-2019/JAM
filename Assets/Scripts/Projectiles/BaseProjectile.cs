using UnityEngine;

namespace Projectiles
{
    public class BaseProjectile : MonoBehaviour
    {
        [Header("Effects")]
        [SerializeField] private GameObject _projectileLaunchEffect;
        [SerializeField] private GameObject _projectileDestroyEffect;

        [Header("Projectile Stats")]
        [SerializeField] private float _launchSpeed = 20;
        [SerializeField] private float _lifetime;

        public float LaunchSpeed => _launchSpeed;

        private float _currentLifeTime;

        #region Unity Event Functions

        private void Start()
        {
            _currentLifeTime = _lifetime;
            SpawnLaunchEffect();
        }

        private void Update()
        {
            _currentLifeTime -= Time.deltaTime;
            if (_currentLifeTime <= 0)
            {
                DestroyProjectile();
            }
        }

        private void OnTriggerEnter2D(Collider2D other) => DestroyProjectile();

        #endregion

        #region Helpers

        private void DestroyProjectile()
        {
            SpawnDestroyEffect();
            Destroy(gameObject);
        }

        private void SpawnLaunchEffect()
        {
            if (_projectileLaunchEffect != null)
            {
                Instantiate(_projectileLaunchEffect, transform.position, Quaternion.identity);
            }
        }

        private void SpawnDestroyEffect()
        {
            if (_projectileDestroyEffect != null)
            {
                Instantiate(_projectileDestroyEffect, transform.position, Quaternion.identity);
            }
        }

        #endregion
    }
}