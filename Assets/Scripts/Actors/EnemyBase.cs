using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JamSpace;


public class EnemyBase : MonoBehaviour
{
    // Considering all enemies have waypoints


    [SerializeField]
    private EEnemyState currentEnemyState;
    public EEnemyState CurrentEnemyState
    {
        set
        {
            currentEnemyState = value;
        }

        get
        {
            return currentEnemyState;
        }
    }

    // all the functionalities of the character
    // such as collider box
    // waypoint following behaviour and all other references goes here

    public virtual void Resting()
    {
        // Set off all the base functionalities of this character
        // Character stays stationary
    }

    public virtual void Patroling()
    {
        // Set on all the base functionalities of this character
        // Character is activated and is now partolling the platform
    }

    // Investigate and thus identify player position
    public virtual void Investigate()
    {
        // do some investigation to idenify the point
        // in the end charge towards the enemy
        // the executed charge action will determine the 
        // way particular enemy attacks player
        CurrentEnemyState = EEnemyState.Charging;
    }


    public virtual void Cooldown()
    {

    }

    public virtual void Charging()
    {

    }

    // Execute the changes in the enemy state
    public virtual void ExecuteEnemyState(EEnemyState _enemyState)
    {
        switch (_enemyState)
        {
            case EEnemyState.NONE:
                break;

            case EEnemyState.Resting:
                Resting();
                break;
            case EEnemyState.Patroling:
                Patroling();
                break;
            case EEnemyState.Investigate:
                Investigate();
                break;
            case EEnemyState.Charging:
                Charging();
                break;
            case EEnemyState.Cooldown:
                Cooldown();
                break;

            case EEnemyState.MAX:
                break;
            default:
                break;
        }
    }
}
