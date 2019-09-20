using Collectibles;
using Player.General;
using SaveSystem;
using UnityEngine;
using Utils;

namespace Interactibles.ColliderModifier
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectBowOnContact : MonoBehaviour
    {
        [SerializeField] private Transform _playerSafePosition;
        [SerializeField] private CollectiblesCollector _collectiblesCollector;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerSaveHelper _playerSaveHelper;
        [SerializeField] private GameObject[] _nearestEnemies;

        #region Unity Functions

        private void Start() => CheckAndLoadData();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                other.GetComponent<PlayerController>().PlayerCollectBow();

                _playerSaveHelper.SavePlayerWithSafePosition(_playerSafePosition.position);
                SaveManager.Instance.SaveStructure.bowCollected = true;
                SaveManager.Instance.SaveData();

                GetComponent<Collider2D>().enabled = false;
            }
        }

        #endregion

        #region Utility Functions

        private void CheckAndLoadData()
        {
            bool bowCollected = SaveManager.Instance.SaveStructure.bowCollected;
            if (!bowCollected)
            {
                return;
            }

            for (int i = 0; i < _nearestEnemies.Length; i++)
            {
                Destroy(_nearestEnemies[i]);
            }

            _playerController.PlayerCollectBow();

            GetComponent<Collider2D>().enabled = false;
            _collectiblesCollector.ClearCollectibleItems();
        }

        #endregion
    }
}