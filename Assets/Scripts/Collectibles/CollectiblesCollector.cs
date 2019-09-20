using Interactibles.ColliderModifier;
using UnityEngine;
using Utils;

namespace Collectibles
{
    public class CollectiblesCollector : MonoBehaviour
    {
        [SerializeField] private CollisionNotifier _collectibleNotifier;
        public ParticleSystem _collectorEffect;

        private bool _deActiveCollector;

        #region Unity Functions

        private void Start()
        {
            if (_deActiveCollector)
            {
                return;
            }

            _collectibleNotifier.OnTriggerEntered += HandlePlayerCollision;
        }

        #endregion

        #region External Functions

        public void ClearCollectibleItems()
        {
            _collectibleNotifier.OnTriggerEntered -= HandlePlayerCollision;

            Destroy(_collectibleNotifier.gameObject);
            _collectorEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);

            _deActiveCollector = true;
        }

        #endregion

        #region Utility Functions

        private void HandlePlayerCollision(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                ClearCollectibleItems();
            }
        }

        #endregion
    }
}