using Player.General;
using UnityEngine;
using Utils;

namespace Interactibles.ColliderModifier
{
    public class LoseDashOnContact : MonoBehaviour
    {
        #region Unity Functions

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                other.GetComponent<PlayerController>().PlayerLoseDash();
            }
        }

        #endregion
    }
}
