using System;
using UnityEngine;

namespace Scenes.Main.Enemy
{
    public class EnemyDeathBagDrop : EnemyDeathController
    {
        [SerializeField] private Rigidbody2D _bag;

        protected override void RunEffectOnZero()
        {
            _bag.isKinematic = false;
            _bag.transform.SetParent(null);

            base.RunEffectOnZero();
        }
    }
}