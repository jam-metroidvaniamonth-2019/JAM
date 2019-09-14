using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityA : BaseBossAbility
{
    [SerializeField]
    private int currentCounter;

    private IEnumerator FireProjectileRoutine(Vector2 _direction)
    {
        LaunchAbilityAProjectile(_direction);
        yield return new WaitForSeconds(delayPerProjectile);
        if (currentCounter > 0)
        {
            StartCoroutine(FireProjectileRoutine(_direction));
            --currentCounter;
        }
        else {
        NotifyAbilityCompleted();
        }
    }

    public float delayPerProjectile;
    public int counter;

    public GameObject projectileA;
    public Vector2 direction;
    public float speed;

    public override void Trigger(Vector2 _direction)
    {
        base.Trigger(_direction);
        currentCounter = counter;
        StartCoroutine(FireProjectileRoutine(_direction));
    }
    private void LaunchAbilityAProjectile(Vector2 _direction)
    {
        var zProjectile = 5;
        var pos = this.transform.position;
        var newPos = new Vector3(pos.x, pos.y, zProjectile);

        var baseEnemyProj = Instantiate(projectileA, newPos, Quaternion.identity).GetComponent<Projectiles.BaseProjectile>();
        baseEnemyProj.GetComponent<Rigidbody2D>().velocity = _direction * speed;
    }
}


