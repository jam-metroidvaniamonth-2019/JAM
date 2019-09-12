using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseBossAbility : MonoBehaviour
{
    public EAttackType attackType;
    public float cooldown;

    public delegate void AbilityCompleted(float cooldownDuration);
    public AbilityCompleted OnAbilityCompleted;
    public void NotifyAbilityCompleted() => OnAbilityCompleted?.Invoke(cooldown);

    public virtual void Trigger(Vector2 _direction)
    {
            
    }
}


