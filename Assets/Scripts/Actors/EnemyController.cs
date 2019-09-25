using Common;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private EnemyHealthSetter _enemyHealthSetter;
    [SerializeField] private GameObject _enemyDeathEffect;
    [SerializeField] private GameObject _enemyBody;

    #region Unity Functions

    private void Start() => _enemyHealthSetter.OnHealthZero += HandleEnemyHealthZero;

    #endregion

    #region External Functions

    public void ReviveEnemy()
    {
        float enemyMaxHealth = _enemyHealthSetter.MaxHealth;
        _enemyHealthSetter.SetCurrentHealth(enemyMaxHealth);

        _enemyBody.SetActive(true);
    }

    #endregion

    #region Utility Functions

    private void HandleEnemyHealthZero()
    {
        _enemyHealthSetter.OnHealthZero -= HandleEnemyHealthZero;

        RunEffectOnZero();
    }

    protected virtual void RunEffectOnZero()
    {
        Instantiate(_enemyDeathEffect, _enemyBody.transform.position, Quaternion.identity);
        _enemyBody.SetActive(false);
    }

    #endregion
}