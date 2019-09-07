using System.Collections;
using UnityEngine;

namespace Common
{
    public class HealthSetter : MonoBehaviour
    {
        public IEnumerator DealInstantaneousDamageRoutine(float _damage, float _time)
        {
            if (bCanDealDamage)
            {
                ReduceHealth(_damage);
            }
            bCanDealDamage = false;
            yield return new WaitForSeconds(_time);
            bCanDealDamage = true;
        }

        public void DealInstantaneousDamage(float _damage,float _time)
        {
            StartCoroutine(DealInstantaneousDamageRoutine(_damage, _time));
        }

        public void AllowDamage()
        {
            StartCoroutine(AllowDamageCoroutine(1f));
        }

        public IEnumerator AllowDamageCoroutine(float _time)
        {
            //bCanDealDamage = false;
            yield return new WaitForSeconds(_time);
            bCanDealDamage = true;
        }

        [SerializeField]
        private bool _canDealDamage = true;

        [SerializeField]
        public bool bCanDealDamage
        {
            get
            {
                return _canDealDamage;
            }

            set
            {
                _canDealDamage = value;
            }
        }

        [SerializeField] private float _maxHealth;

        public delegate void HealthZero();
        public HealthZero OnHealthZero;

        public delegate void HealthChanged(float currentHealth, float maxHealth);
        public HealthChanged OnHealthChanged;

        private bool _zeroHealthNotified;
        [SerializeField]
        private float _currentHealth;

        #region Unity Functions

        private void Start() => _currentHealth = _maxHealth;

        #endregion

        #region External Functions

        public void ReduceHealth(float reductionAmount)
        {
            _currentHealth -= reductionAmount;
            if (_currentHealth <= 0 && !_zeroHealthNotified)
            {
                _zeroHealthNotified = true;
                OnHealthZero?.Invoke();
            }

            NotifyHealthChanged();
            AllowDamage();
        }

        public void IncreaseHealth(float incrementAmount)
        {
            _currentHealth += incrementAmount;
            if (_currentHealth > _maxHealth)
            {
                _currentHealth = _maxHealth;
            }

            NotifyHealthChanged();
        }

        #endregion

        #region Utility Functions

        private void NotifyHealthChanged() => OnHealthChanged?.Invoke(_currentHealth, _maxHealth);

        #endregion
    }
}