using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityB : BaseBossAbility
{
    public Transform FirePt;


    public GameObject projectileB;
    public Vector2 direction;
    public float speed;
    public float child_speed;
    public float blastTimeSinceSpawn;

    public override void Trigger(Vector2 _direction)
    {
        base.Trigger(_direction);
        LaunchAbilityBProjectile(_direction);
        NotifyAbilityCompleted();
    }

    private void LaunchAbilityBProjectile(Vector2 _direction)
    {
        var zProjectile = 0;
        var pos = FirePt.transform.position;
        var newPos = new Vector3(pos.x, pos.y, zProjectile);

        var baseEnemyProj = Instantiate(projectileB, newPos, Quaternion.identity).GetComponent<ProjectileB_Primary>();
        baseEnemyProj.GetComponent<Rigidbody2D>().velocity = _direction * speed;
        baseEnemyProj.child_speed = child_speed;
        baseEnemyProj.child_damage = baseEnemyProj.GetComponent<Common.AffectorAmount>().Amount / (baseEnemyProj.pointsForChildProjectils.Count);
    }
}
