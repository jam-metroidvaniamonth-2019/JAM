using UnityEngine;
using WorldDisplay;

namespace Player.Movement
{
    public class ExplosionCollisionTrigger : MonoBehaviour
    {
        [SerializeField] private string _targetTag;
        [SerializeField] private TriggerExplosionOnChildren _explosionTrigger;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag(_targetTag))
            {
                return;
            }

            _explosionTrigger.MakeChildrenExplode();
            gameObject.SetActive(false);
        }
    }
}