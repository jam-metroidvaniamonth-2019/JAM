using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingComponent : MonoBehaviour
{
    public Common.HealthSetter playerHealthSetter;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var _validHealthSetter = collision.GetComponent<Common.HealthSetter>();

        if (_validHealthSetter)
        {
            if (!playerHealthSetter)
            {
                playerHealthSetter = _validHealthSetter;
            }
        }       
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var _validHealthSetter = collision.GetComponent<Common.HealthSetter>();

        if (_validHealthSetter)
        {
            if (playerHealthSetter)
            {
                playerHealthSetter = null;
            }
        }
    }

    public bool DealDamage(float _damageValue)
    {
        if (playerHealthSetter)
        {
            playerHealthSetter.ReduceHealth(_damageValue);
            return true;
        }
        else
        {
            return false;
        }
    }
}
