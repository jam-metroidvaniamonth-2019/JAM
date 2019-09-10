using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StagnantEnemy : BaseNPC
{

    public bool bIsInCooldown;
    public float cooldownTimer;
    public float cooldownCounter;


    [SerializeField]
    private GameObject projectilePrefab;

    [SerializeField]
    private float rangeRadius;

    [SerializeField]
    private float playerInsideDur;

    [SerializeField]
    private float readyToAttackDur;

    [SerializeField]
    private Collider2D monitoringRange;

    [SerializeField]
    private float cooldownDur;

    [SerializeField]
    private float readyToCoolDur;

    [SerializeField]
    Animator myAnimator;

    [SerializeField]
    JamSpace.EState currentEState;

    public void TriggerAnimation(string animTag)
    {
        return;
        myAnimator.SetTrigger(animTag);
    }

    public JamSpace.EState CurrentState
    {
        get
        {
            return currentEState;
        }
        set
        {
            currentEState = value;
            ExecuteState(currentEState);
        }
    }

    IEnumerator CallCooldown()
    {
        TriggerAnimation(JamSpace.AnimationTags.ANIMATION_COOLDOWN);
        yield return new WaitForSeconds(cooldownDur);
        CurrentState = JamSpace.EState.IDLE;
    }

    IEnumerator CallMonitoring()
    {
        TriggerAnimation(JamSpace.AnimationTags.ANIMATION_INVESTIGATE);
        playerInsideRangeCounter = readyToAttackDur;
        yield return new WaitForSeconds(readyToAttackDur);

        if (InvestigatedTargetHealthSetter)
        {
            CurrentState = JamSpace.EState.ATTACKING;
        }
        else
        {
            CurrentState = JamSpace.EState.IDLE;
        }
    }

    public Common.HealthSetter InvestigatedTargetHealthSetter = null;

    private bool bWasLastRecorededInside = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bWasLastRecorededInside = true;

    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!bIsInCooldown)
        {
            var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
            if (collisionScript)
            {
                InvestigatedTargetHealthSetter = collisionScript.GetComponent<Common.HealthSetter>();

                if (bCanStartCounter)
                {
                    CurrentState = JamSpace.EState.MONITORING;
                    bCanStartCounter = false;
                }
            }
        }
    }

    private void Update()
    {
        if (bIsInCooldown)
        {
            cooldownCounter += Time.deltaTime;
            if(cooldownCounter>= cooldownTimer)
            {
                cooldownCounter = 0;
                bIsInCooldown = false;
                if (InvestigatedTargetHealthSetter != null)
                {
                    InvestigatedTargetHealthSetter.GetComponent<Player.Movement.PlayerCollision>().GetComponent<Rigidbody2D>().WakeUp();
                }
            }
        }

    }

    [Header("Counter")]
    [SerializeField]
    private bool bCanStartCounter = false;
    [SerializeField]
    private float playerInsideRangeCounter;

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
        if (collisionScript)
        {
            InvestigatedTargetHealthSetter = null;
        }
    }

    public void Attack()
    {
        var playerXPos = InvestigatedTargetHealthSetter.transform.position.x;
        var myXPos = this.transform.position.x;
        Projectiles.BaseProjectile baseEnemyProj = null;

        if (((playerXPos - myXPos) < 0))
        {
            baseEnemyProj = Instantiate(projectilePrefab, this.transform.position, Quaternion.Euler(0, -180, 0)).GetComponent<Projectiles.BaseProjectile>();
            baseEnemyProj.GetComponent<Rigidbody2D>().velocity = speed * baseEnemyProj.transform.right;
        }
        else
        {
            baseEnemyProj = Instantiate(projectilePrefab, this.transform.position, Quaternion.Euler(0, 0, 0)).GetComponent<Projectiles.BaseProjectile>();
            baseEnemyProj.GetComponent<Rigidbody2D>().velocity = speed * baseEnemyProj.transform.right;
        }

        bIsInCooldown = true;
        CurrentState = JamSpace.EState.IDLE;
    }

    private void Start()
    {
        this.gameObject.GetComponent<CircleCollider2D>().radius = rangeRadius;
    }

    void ExecuteState(JamSpace.EState _enemyState)
    {
        switch (_enemyState)
        {
            case JamSpace.EState.NONE:
                break;
            case JamSpace.EState.IDLE:
                bCanStartCounter = true;
                TriggerAnimation(JamSpace.AnimationTags.ANIMATION_IDLE);
                break;
            case JamSpace.EState.MONITORING:
                StartCoroutine(CallMonitoring());
                break;
            case JamSpace.EState.ATTACKING:
                Attack();
                break;
            case JamSpace.EState.COOLDOWN:
                StartCoroutine(CallCooldown());
                break;
            case JamSpace.EState.MAX:
                break;
            default:
                break;
        }
    }
}
