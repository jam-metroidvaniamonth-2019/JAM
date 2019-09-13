using UnityEngine;
using Utils;

namespace Interactibles.ColliderModifier
{
    public class SpawnEffectOnCollision : MonoBehaviour
    {
        [SerializeField] private GameObject _effect;
        [SerializeField] private bool _detectCollisionOnOnce;
        [SerializeField] private bool _detectOnlyPlayer;

        private bool _objectCollided;

        #region Unity Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (_objectCollided && _detectCollisionOnOnce)
            {
                return;
            }

            if (_detectOnlyPlayer)
            {
                if (other.CompareTag(TagManager.Player))
                {
                    SpawnEffect();
                    _objectCollided = true;
                }
            }
            else
            {
                SpawnEffect();
                _objectCollided = true;
            }
        }

        #endregion

        #region Utility Functions

        private void SpawnEffect() => Instantiate(_effect, transform.position, Quaternion.identity);

        #endregion
    }
}