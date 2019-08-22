using UnityEngine;

namespace Common
{
    public class DamageAmountSetter : MonoBehaviour
    {
        [SerializeField] private float _damageAmount;

        public float DamageAmount => _damageAmount;
    }
}