using Collectibles;
using Player.General;
using SaveSystem;
using SpeechSystem;
using UnityEngine;
using Utils;

namespace Interactibles.ColliderModifier
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectBowOnContact : MonoBehaviour
    {
        [SerializeField] private Transform _playerSafePosition;
        [SerializeField] private PlayerSaveHelper _playerSaveHelper;
        [SerializeField] private EnemyController[] _enemyControllers;

        #region Unity Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                other.GetComponent<PlayerController>().PlayerCollectBow();

                _playerSaveHelper.SavePlayerWithSafePosition(_playerSafePosition.position);
                SaveManager.Instance.SaveStructure.bowCollected = true;
                SaveManager.Instance.SaveStructure.enemyControllers = _enemyControllers;
                SaveManager.Instance.SaveData();

                GetComponent<Collider2D>().enabled = false;
            }
        }

        #endregion
    }
}