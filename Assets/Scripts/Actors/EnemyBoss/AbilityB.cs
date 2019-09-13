using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityB : BaseBossAbility
{
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
        var baseEnemyProj = Instantiate(projectileB, this.transform.position, Quaternion.identity).GetComponent<ProjectileB_Primary>();
        baseEnemyProj.GetComponent<Rigidbody2D>().velocity = _direction * speed;
        baseEnemyProj.child_speed = child_speed;
        baseEnemyProj.child_damage = baseEnemyProj.GetComponent<Common.AffectorAmount>().Amount / (baseEnemyProj.pointsForChildProjectils.Count);
    }
}
