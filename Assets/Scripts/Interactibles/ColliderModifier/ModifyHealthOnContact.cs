using Common;
using UnityEngine;
using Utils;

namespace Interactibles.ColliderModifier
{
    public class ModifyHealthOnContact : MonoBehaviour
    {
        [SerializeField] private bool _increaseHealth;
        [SerializeField] private bool _targetPlayer;

        private AffectorAmount _affector;

        #region Unity Functions

        private void Start() => _affector = GetComponent<AffectorAmount>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            HealthSetter healthSetter = other.GetComponent<HealthSetter>();
            if (!healthSetter)
            {
                return;
            }

            if (_increaseHealth)
            {
                IncreaseHealthOnContact(healthSetter, other.gameObject);
            }
            else
            {
                DecreaseHealthOnContact(healthSetter, other.gameObject);
            }
        }

        #endregion

        #region Utility Functions

        private void IncreaseHealthOnContact(HealthSetter healthSetter, GameObject other)
        {
            if (_targetPlayer)
            {
                if (!other.CompareTag(TagManager.Player))
                {
                    return;
                }

                healthSetter.IncreaseHealth(_affector.Amount);
            }
            else
            {
                healthSetter.IncreaseHealth(_affector.Amount);
            }
        }

        private void DecreaseHealthOnContact(HealthSetter healthSetter, GameObject other)
        {
            if (_targetPlayer)
            {
                if (!other.CompareTag(TagManager.Player))
                {
                    return;
                }

                healthSetter.ReduceHealth(_affector.Amount);
            }
            else
            {
                healthSetter.ReduceHealth(_affector.Amount);
            }
        }

        #endregion
    }
}