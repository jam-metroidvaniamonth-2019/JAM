using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPersona : MonoBehaviour
{
    [SerializeField]
    private EnemyMovement enemyMovement;


    public delegate void EnemyObserved(GameObject observedEnemy);
    public EnemyObserved OnEnemyObserved;

    private void Start()
    {
        OnEnemyObserved += AttackEnemy;
    }

    private void AttackEnemy(GameObject _enemyObject)
    {
        if (enemyMovement.bAllowTargetUpdate)
        {
            StartCoroutine(enemyMovement.AllowTargetUpdate(1f));
            enemyMovement.targetPoint = _enemyObject.transform.position;
        }

    }
}
