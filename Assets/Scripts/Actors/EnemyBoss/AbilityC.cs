using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityC : BaseBossAbility
{
    public override void Trigger(Vector2 _direction)
    {
        base.Trigger(_direction);
        NotifyAbilityCompleted();

    }

}
