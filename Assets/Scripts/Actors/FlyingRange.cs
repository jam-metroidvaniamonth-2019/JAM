using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingRange : MonoBehaviour
{
    public float baseAnim_Atttack_Time = 0.4677f;
    public float baseAnim_Monitoring_Time = 0.4f;
    public float baseAnim_Idle_Time = 0.4f;

    public bool isInCooldown;
    public float cooldownTimer;
    public float cooldownCounter;

    [SerializeField]
    float normalSpeed;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    private bool isMovingRight;
    private const float rotationEulerAngle = 180;

    private void EvaluateRotation()
    {
        if (isMovingRight)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
    }
    private void Update()
    {
        if (isInCooldown)
        {
            cooldownCounter += Time.deltaTime;
            if(cooldownCounter>= cooldownTimer)
            {
                cooldownCounter = 0;
                isInCooldown = false;
            }
        }

        Move();

    }
    private Vector2 GetRandomPosititonBetweenPoints(ref Transform point1, ref Transform point2)
    {
        float randomXValue = 0f;
        randomXValue = Random.Range(point1.position.x, point2.position.x);
        return new Vector2(randomXValue, point2.position.y);
    }
    private void Move()
    {
        transform.position =
            Vector2.MoveTowards(transform.position, targetPoint, moveSpeed * Time.deltaTime);

        if (Mathf.Approximately(transform.position.x, targetPoint.x) &&
            Mathf.Approximately(transform.position.y, targetPoint.y))
        {
            GetNextPoint();
        }
    }
    private void GetNextPoint()
    {
        if (Mathf.Approximately(point1.position.x, targetPoint.x) &&
            Mathf.Approximately(point1.position.y, targetPoint.y))
        {
            targetPoint = point2.transform.position;
            isMovingRight = !isMovingRight;
            EvaluateRotation();
        }
        else if (Mathf.Approximately(point2.position.x, targetPoint.x) &&
            Mathf.Approximately(point2.position.y, targetPoint.y))
        {
            targetPoint = point1.transform.position;
            isMovingRight = !isMovingRight;
            EvaluateRotation();
        }
        else
        {

        }

        moveSpeed = normalSpeed;
        if (currentEState == JamSpace.EState.ATTACKING)
        {
            CurrentState = JamSpace.EState.COOLDOWN;
        }
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

    [SerializeField]
    Animator myAnimator;

    [Header("Wait")]
    [SerializeField]
    float Wait_Cooldown;
    [SerializeField]
    float Wait_Monitoring;

    [SerializeField]
    float wait_attack_position;

    void TriggerAnimation(string animTag)
    {
        myAnimator.SetTrigger(animTag);
    }

    public Common.HealthSetter InvestigatedTargetHealthSetter = null;

    [SerializeField]
    public GameObject projectilePrefab;
    [SerializeField]
    private float speed;

    public void FireProjectileBlast(Vector3 _playerPosition)
    {
        var orbDirection = (_playerPosition - this.transform.position).normalized;
        var baseEnemyProj = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity).GetComponent<Projectiles.BaseProjectile>();
        baseEnemyProj.GetComponent<Rigidbody2D>().velocity =  orbDirection * speed;

        orbDirection = Quaternion.Euler(0, 180, 0) * orbDirection;
        baseEnemyProj = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity).GetComponent<Projectiles.BaseProjectile>();
        baseEnemyProj.GetComponent<Rigidbody2D>().velocity = orbDirection * speed;

        orbDirection = Quaternion.Euler(0, 180, 0) * orbDirection;
        baseEnemyProj = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity).GetComponent<Projectiles.BaseProjectile>();
        baseEnemyProj.GetComponent<Rigidbody2D>().velocity = orbDirection * -speed;

        orbDirection = Quaternion.Euler(0, 180, 0) * orbDirection;
        baseEnemyProj = Instantiate(projectilePrefab, this.transform.position, Quaternion.identity).GetComponent<Projectiles.BaseProjectile>();
        baseEnemyProj.GetComponent<Rigidbody2D>().velocity = orbDirection * -speed;
    }

    IEnumerator CallMonitoring()
    {
        moveSpeed = 0;
        TriggerAnimation(JamSpace.AnimationTags.ANIMATION_INVESTIGATE);
        myAnimator.speed = (1 / (Wait_Monitoring / baseAnim_Monitoring_Time));
        yield return new WaitForSeconds(Wait_Monitoring);
        if (InvestigatedTargetHealthSetter)
        {
            // fire projectile blast and continue
            CurrentState = JamSpace.EState.ATTACKING;
        }
        else
        {
            CurrentState = JamSpace.EState.IDLE;
        }
    }

    IEnumerator CallAttack()
    {
        TriggerAnimation(JamSpace.AnimationTags.ANIMATION_CHARGE);
        yield return new WaitForSeconds(wait_attack_position);
        moveSpeed = normalSpeed;
        CurrentState = JamSpace.EState.IDLE;
    }

    private void Attack()
    {
        FireProjectileBlast(InvestigatedTargetHealthSetter.transform.position);
        TriggerAnimation(JamSpace.AnimationTags.ANIMATION_CHARGE);
        moveSpeed = normalSpeed;
        CurrentState = JamSpace.EState.IDLE;
        isInCooldown = true;
        //StartCoroutine(CallAttack());
    }

    private void Start()
    {
        targetPoint = point2.position;
        EvaluateRotation();

        myAnimator = this.GetComponent<Animator>();
    }

    IEnumerator CallCooldown()
    {
        moveSpeed = 0;
        //TriggerAnimation(JamSpace.AnimationTags.ANIMATION_COOLDOWN);

        TriggerAnimation(JamSpace.AnimationTags.ANIMATION_IDLE);
        myAnimator.speed = 1;
        yield return new WaitForSeconds(Wait_Cooldown);
        CurrentState = JamSpace.EState.IDLE;
    }

    void ExecuteState(JamSpace.EState _enemyState)
    {
        switch (_enemyState)
        {
            case JamSpace.EState.NONE:
                break;
            case JamSpace.EState.IDLE:
                moveSpeed = normalSpeed;
                TriggerAnimation(JamSpace.AnimationTags.ANIMATION_IDLE);
                InvestigatedTargetHealthSetter = null;
                myAnimator.speed = 1;
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

    [SerializeField]
    JamSpace.EState currentEState;

    [SerializeField]
    private Transform point1;
    [SerializeField]
    private Transform point2;
    [SerializeField]
    public Vector2 targetPoint;

    private void OnTriggerExit2D(Collider2D collision)
    {
        var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
        if (collisionScript)
        {
            InvestigatedTargetHealthSetter = null;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (CurrentState == JamSpace.EState.IDLE && !isInCooldown)
        {
            var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
            if (collisionScript)
            {
                InvestigatedTargetHealthSetter = collisionScript.GetComponent<Common.HealthSetter>();
                CurrentState = JamSpace.EState.MONITORING;
            }
        }
    }

}
