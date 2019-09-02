using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigatingComponent : MonoBehaviour
{
    public FlyDash parentComponent;
    public Common.HealthSetter InvestigatedTargetHealthSetter = null;

    #region UnityCallbacks

    private void Start()
    {
        //parentComponent = this.GetComponent<FlyDash>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
        if (collisionScript)
        {
            parentComponent.IsEnemyVisible = true;
            InvestigatedTargetHealthSetter = collisionScript.GetComponent<Common.HealthSetter>();
        }    
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
        if (collisionScript)
        {
            parentComponent.IsEnemyVisible = false;
            InvestigatedTargetHealthSetter = null;
        }
    }

    #endregion

}
