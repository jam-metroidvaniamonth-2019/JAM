using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JamSpace;


public class EnemyBase : MonoBehaviour
{
    // Considering all enemies have waypoints
    public Animator myAnimator;

    public CircleCollider2D circleCollider;

    void Attack()
    {

    }

    public float lengthOfWaitForEntryAnimation;
    public float lengthOfWaitBeforeAttackingAfterInvestigation;
    public float lengthOfCooldown;

    public virtual void PlayerInteractOnAttack(Common.HealthSetter _playerHealthSetter, float _damageValue)
    {
        _playerHealthSetter.ReduceHealth(_damageValue);
    }

    private void Start()
    {
        myAnimator = this.GetComponent<Animator>();
        circleCollider = this.GetComponent<CircleCollider2D>();
    }

    protected void CallAnimator(string animTag)
    {
        myAnimator.SetTrigger(animTag);
    }

    public void ActivateEnemy()
    {
        // this will trigger the enemy
        // for ex. as in the first level for Ori and the blind forest
        // the charging rhino 
        // comes out of the mushroom patch from below the ground
        CurrentEnemyState = EEnemyState.Idle;
    }

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
        CallAnimator("Rest");
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
        CallAnimator("Investigate");
        Invoke("Charging", lengthOfWaitBeforeAttackingAfterInvestigation);
    }



    public virtual void Cooldown()
    {
        //Invoke("Resting")
    }

    public virtual void Charging()
    {
        CallAnimator("Charge");
    }

    public virtual void Defeated()
    {
        CallAnimator("Defeated");
        Invoke("HideCharacter", 2f);
    }

    void HideCharacter()
    {
        this.gameObject.SetActive(false);
    }


    public virtual void Entry()
    {
        // call entry animation here
        // after entry animation ends
        // call in animation event for charging state to Resting
        Invoke("Resting()", lengthOfWaitForEntryAnimation);
    }

    

    // Execute the changes in the enemy state
    //public virtual void ExecuteEnemyState(EEnemyState _enemyState)
    //{
    //    switch (_enemyState)
    //    {
    //        case EEnemyState.NONE:
    //            break;
    //        case EEnemyState.Entry:
    //            Entry();
    //            break;

    //        case EEnemyState.Resting:
    //            Resting();
    //            break;
    //        case EEnemyState.Patroling:
    //            Patroling();
    //            break;
    //        case EEnemyState.Investigate:
    //            Investigate();
    //            break;
    //        case EEnemyState.Charging:
    //            Charging();
    //            break;
    //        case EEnemyState.Cooldown:
    //            Cooldown();
    //            break;

    //        case EEnemyState.MAX:
    //            break;
    //        default:
    //            break;
    //    }
    //}
}
