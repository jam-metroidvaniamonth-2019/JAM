using System;
using Common;
using UnityEngine;
using Utils;

namespace Interactibles.ColliderModifier
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class DamageOnCollisionWithVelocity : MonoBehaviour
    {
        [SerializeField] private float _minAffectMagnitude;
        [SerializeField] private bool _affectOnlyPlayer;
        [SerializeField] private float _damageAmount;

        private Rigidbody2D _objectRb;

        #region Unity Functions

        private void Start() => _objectRb = GetComponent<Rigidbody2D>();

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_objectRb.velocity.sqrMagnitude < _minAffectMagnitude)
            {
                return;
            }

            if (_affectOnlyPlayer)
            {
                if (other.gameObject.CompareTag(TagManager.Player))
                {
                    HealthSetter playerHealthSetter = other.gameObject.GetComponent<HealthSetter>();
                    playerHealthSetter.ReduceHealth(_damageAmount);
                }
            }
            else
            {
                HealthSetter healthSetter = other.gameObject.GetComponent<HealthSetter>();
                if (healthSetter != null)
                {
                    healthSetter.ReduceHealth(_damageAmount);
                }
            }
        }

        #endregion
    }
}