using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPC : MonoBehaviour
{
    public EnemyHealthObject enemyHealthObject;

    public virtual void Start()
    {
        enemyHealthObject.enemyHealthSetter.OnHealthZero += EnemyDead;
        enemyHealthObject.enemyCollider = this.GetComponent<Collider2D>();
    }

    public delegate void EnemyDeathPosition(BaseNPC _baseEnemy);
    public EnemyDeathPosition OnEnemyDeathPosition;

    public void EnemyDead()
    {
        OnEnemyDeathPosition(this);
        // this character is dead
    }

    public float detectionDelay;
    public float damageOnTouch;
    public float speed;

    public virtual void Init()
    {
        
    }

    public void DealDamage(ref Common.HealthSetter healthSetter, float _damage)
    {
        healthSetter.ReduceHealth(_damage);
    }
}
