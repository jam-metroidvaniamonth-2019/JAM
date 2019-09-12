using Common;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Display
{
    public class PlayerHealthDisplay : MonoBehaviour
    {
        [Header("Colors")]
        [SerializeField] private Color _minHealthColor = Color.red;
        [SerializeField] private Color _halfHealthColor = Color.yellow;
        [SerializeField] private Color _maxHealthColor = Color.green;

        [Header("Display")]
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private Image _healthFiller;
        [SerializeField] private HealthSetter _healthSetter;

        #region Unity Functions

        private void Start()
        {
            _healthSetter.OnHealthChanged += HandleHealthChange;
        }

        #endregion

        #region Utility Functions

        private void HandleHealthChange(float currentHealth, float maxHealth)
        {
            float healthRatio = currentHealth / maxHealth;

            if (healthRatio <= 0.5f)
            {
                _healthFiller.color = Color.Lerp(_minHealthColor, _halfHealthColor, healthRatio * 2);
            }
            else
            {
                _healthFiller.color = Color.Lerp(_halfHealthColor, _maxHealthColor, (healthRatio - 0.5f) * 2);
            }

            _healthSlider.value = healthRatio;
        }

        #endregion
    }
}