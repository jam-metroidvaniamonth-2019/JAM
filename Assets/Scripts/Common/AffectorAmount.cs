using UnityEngine;

namespace Common
{
    public class AffectorAmount : MonoBehaviour
    {
        [SerializeField] private float _amount;

        public float Amount => _amount;

        public void SetDamage(float _dmg)
        {
            _amount = _dmg;
        }
    }
}