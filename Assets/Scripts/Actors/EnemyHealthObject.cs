using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthObject : MonoBehaviour
{
    public Common.EnemyHealthSetter enemyHealthSetter;
    public Collider2D enemyCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var coll = collision.GetComponent<Projectiles.BaseProjectile>();
        if (coll)
        {
            enemyHealthSetter.ReduceHealth(coll.GetComponent<Common.AffectorAmount>().Amount);
        }
    }
}
