using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityA : BaseBossAbility
{
    public Transform FirePt;

    public Transform playerTransform;

    [SerializeField]
    private int currentCounter;

    private IEnumerator FireProjectileRoutine()
    {
        var _direction = new Vector3(0, 0, 0);
        _direction = (playerTransform.position - FirePt.position).normalized;

        LaunchAbilityAProjectile(_direction);
        yield return new WaitForSeconds(delayPerProjectile);
        if (currentCounter > 0)
        {
            StartCoroutine(FireProjectileRoutine());
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

    public void TriggerA(Transform _transform)
    {
        currentCounter = counter;
        playerTransform = _transform;
        StartCoroutine(FireProjectileRoutine());
    }

    public override void Trigger(Vector2 _direction)
    {
        base.Trigger(_direction);
        
    }
    private void LaunchAbilityAProjectile(Vector2 _direction)
    {
        var zProjectile = 0;
        var pos = FirePt.transform.position;
        var newPos = new Vector3(pos.x, pos.y, zProjectile);

        var baseEnemyProj = Instantiate(projectileA, newPos, Quaternion.identity).GetComponent<Projectiles.BaseProjectile>();
        baseEnemyProj.GetComponent<Rigidbody2D>().velocity = _direction * speed;
    }
}


