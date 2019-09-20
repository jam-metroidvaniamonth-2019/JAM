using Collectibles;
using Player.General;
using SaveSystem;
using SpeechSystem;
using UnityEngine;
using Utils;

namespace Interactibles.ColliderModifier
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectDashOnContact : MonoBehaviour
    {
        [SerializeField] private Transform _safePosition;
        [SerializeField] private CollectiblesCollector _collectiblesCollector;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerSaveHelper _playerSaveHelper;
        [SerializeField] private GameObject[] _nearestEnemies;
        [SerializeField] private SpeechTrigger[] _speechTriggers;

        #region Unity Functions

        private void Start() => CheckAndLoadData();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                other.GetComponent<PlayerController>().PlayerCollectDash();

                _playerSaveHelper.SavePlayerWithSafePosition(_safePosition.position);
                SaveManager.Instance.SaveStructure.dashCollected = true;
                SaveManager.Instance.SaveData();

                GetComponent<Collider2D>().enabled = false;
            }
        }

        #endregion

        #region Utility Functions

        private void CheckAndLoadData()
        {
            bool dashCollected = SaveManager.Instance.SaveStructure.dashCollected;
            if (!dashCollected)
            {
                return;
            }

            for (int i = 0; i < _nearestEnemies.Length; i++)
            {
                Destroy(_nearestEnemies[i]);
            }

            for (int i = 0; i < _speechTriggers.Length; i++)
            {
                _speechTriggers[i].DisableSpeechTrigger();
            }

            _playerController.PlayerCollectDash();

            GetComponent<Collider2D>().enabled = false;
            _collectiblesCollector.ClearCollectibleItems();
        }

        #endregion
    }
}
