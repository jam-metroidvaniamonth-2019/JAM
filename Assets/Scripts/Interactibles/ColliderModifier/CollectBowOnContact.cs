using Player.General;
using UnityEngine;
using Utils;

namespace Interactibles.ColliderModifier
{
    [RequireComponent(typeof(Collider2D))]
    public class CollectBowOnContact : MonoBehaviour
    {
        #region Unity Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                other.GetComponent<PlayerController>().PlayerCollectBow();
            }
        }

        #endregion
    }
}