using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySkin : MonoBehaviour
{
    // damage on touching the enemy
    [SerializeField]
    private float damage;
    [SerializeField]
    private Collider2D enemyCollider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
        if (collisionScript)
        {
            var healthSetter =
            collisionScript.GetComponent<Common.HealthSetter>();
            healthSetter.DealInstantaneousDamage(damage, 1f);
        }
    }

}
