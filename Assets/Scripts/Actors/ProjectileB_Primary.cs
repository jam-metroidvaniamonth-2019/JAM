using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileB_Primary : BaseEnemyProjectile
{
    public List<Transform> pointsForChildProjectils;
    public GameObject childProjectile;
    public float child_speed;
    public float child_damage;

    [SerializeField]
    private float blastLifeTime;

    public override void Update()
    {
        blastLifeTime -= Time.deltaTime;
        if (blastLifeTime <= 0)
        {
            BlastProjectileToReleaseSecondaryProjectile();
        }
    }

    public void BlastProjectileToReleaseSecondaryProjectile()
    {


        for (int i0 = 0; i0 < pointsForChildProjectils.Count; i0++)
        {
            var zProjectile = 5;
            var pos = pointsForChildProjectils[i0].position;
            var newPos = new Vector3(pos.x, pos.y, zProjectile);

            var baseEnemyProj = Instantiate(childProjectile, pos, Quaternion.identity).GetComponent<Projectiles.BaseProjectile>();
            baseEnemyProj.GetComponent<Common.AffectorAmount>().SetDamage(child_damage);
            baseEnemyProj.GetComponent<Rigidbody2D>().velocity = this.GetComponent<Rigidbody2D>().velocity * -3;
        }

        DestroyProjectile();
    }
}
