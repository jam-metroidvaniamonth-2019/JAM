using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseNPC : MonoBehaviour
{
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
