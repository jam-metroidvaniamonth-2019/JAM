using Interactibles.ColliderModifier;
using Interactibles.Followers;
using UnityEngine;
using Utils;

namespace Scenes.Main
{
    public class PlayerFireflyInitialInteraction : MonoBehaviour
    {
        [SerializeField] private CollisionNotifier _playerInitialContactPoint;
        [SerializeField] private CollisionNotifier _playerFinalContactPoint;

        private int _fireflyIndex;

        #region Unity Functions

        private void Start()
        {
            _playerInitialContactPoint.OnTriggerEntered += HandlePlayerContactFirstPoint;
            _playerFinalContactPoint.OnTriggerEntered += HandlePlayerContactFinalPoint;

            SpawnInitialFirefly();
        }

        private void OnDestroy()
        {
            _playerInitialContactPoint.OnTriggerEntered -= HandlePlayerContactFirstPoint;
            _playerFinalContactPoint.OnTriggerEntered -= HandlePlayerContactFinalPoint;
        }

        #endregion

        #region Utility Functions

        private void SpawnInitialFirefly()
        {
            Transform fireflySpawnPoint = _playerInitialContactPoint.transform;
            _fireflyIndex = FireflyManager.Instance.AddFirefly(false, fireflySpawnPoint.position, fireflySpawnPoint);
        }

        private void HandlePlayerContactFirstPoint(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                FireflyManager.Instance.UpdateFireflyTarget(other.transform, _fireflyIndex);
            }
        }

        private void HandlePlayerContactFinalPoint(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                FireflyManager.Instance.UpdateFireflyTargetToRandom(_fireflyIndex, true);
            }
        }

        #endregion
    }
}