using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMonitor : MonoBehaviour
{
    [SerializeField]
    private Collider2D monitoringRange;
    [SerializeField]
    public EnemyPersona myPersona;

    [SerializeField]
    private GameObject observedEnemyObject;

    private void NotifyObservedEnemy() => myPersona.OnEnemyObserved?.Invoke(observedEnemyObject);
    private void Start()
    {
        monitoringRange = this.GetComponent<Collider2D>();
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        // acquire target
        var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
        if (collisionScript)
        {
            observedEnemyObject = collisionScript.gameObject;
            NotifyObservedEnemy();
        }
    }

}
