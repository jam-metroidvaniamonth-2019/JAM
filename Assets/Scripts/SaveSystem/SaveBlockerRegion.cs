using UnityEngine;
using Utils;

namespace SaveSystem
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class SaveBlockerRegion : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                SaveManager.Instance.DisableSaving();
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(TagManager.Player))
            {
                SaveManager.Instance.EnableSaving();
            }
        }
    }
}