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
        [SerializeField] private PlayerSaveHelper _playerSaveHelper;

        #region Unity Functions

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
    }
}
