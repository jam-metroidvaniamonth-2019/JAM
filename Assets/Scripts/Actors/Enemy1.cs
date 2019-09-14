using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : BaseNPC
{
    public bool isGroundCharging;

    public float baseAnim_Atttack_Time = 0.4677f;
    public float baseAnim_Monitoring_Time = 0.4f;
    public float baseAnim_Idle_Time = 0.4f;

    public bool isEnemy1;

    public string groundMask = "Ground";

    [Header("Wait")]
    [SerializeField]
    float Wait_Cooldown;
    [SerializeField]
    float Wait_Monitoring;

    [Header("State")]

    [SerializeField]
    JamSpace.EState currentEState;
    [SerializeField]
    Animator myAnimator;
    [SerializeField]
    Collider2D boxCollider2d;

    // ability to sense presence

    [SerializeField]
    private bool isMovingRight;
    private const float rotationEulerAngle = 180;
    private void EvaluateRotation()
    {
        if ((this.gameObject.transform.position.x - targetPoint.x) > 0)
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    void Start()
    {
        targetPoint = point2.position;
        EvaluateRotation();
        //transform.eulerAngles = new Vector3(0, -rotationEulerAngle, 0);
        //int intLMask = LayerMask.GetMask(groundMask);
        myAnimator = this.GetComponent<Animator>();
    }
    void TriggerAnimation(string animTag)
    {
        
        myAnimator.SetTrigger(animTag);
    }
    // Call on enemy activation event
    public override void Init()
    {
        CurrentState = JamSpace.EState.IDLE;
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

    // experimental
    [SerializeField]
    float moveSpeed;

    [SerializeField]
    bool moveRight;
    [SerializeField]
    float chargeSpeed;
    [SerializeField]
    float normalSpeed;
    public float rayCastDistance;
    public Common.HealthSetter InvestigatedTargetHealthSetter = null;

    public float _counter;
    public float _waitAfterEndPoint;
    public bool _canSearch;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_canSearch)
        {
            var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
            if (collisionScript)
            {
                InvestigatedTargetHealthSetter = collisionScript.GetComponent<Common.HealthSetter>();
                CurrentState = JamSpace.EState.MONITORING;
                StopFromSearch();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var collisionScript = collision.GetComponent<Player.Movement.PlayerCollision>();
        if (collisionScript)
        {
            InvestigatedTargetHealthSetter = null;
        }
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
    void Attack()
    {
        moveSpeed = chargeSpeed;
        TriggerAnimation(JamSpace.AnimationTags.ANIMATION_CHARGE);
        
    }
    IEnumerator CallCooldown()
    {
        moveSpeed = normalSpeed;
        //TriggerAnimation(JamSpace.AnimationTags.ANIMATION_COOLDOWN);

        TriggerAnimation(JamSpace.AnimationTags.ANIMATION_IDLE);
        myAnimator.speed = 1;
        yield return new WaitForSeconds(Wait_Cooldown);
        CurrentState = JamSpace.EState.IDLE;
    }
    IEnumerator CallMonitoring()
    {
        moveSpeed = 0;
        TriggerAnimation(JamSpace.AnimationTags.ANIMATION_INVESTIGATE);
        myAnimator.speed = (1/(Wait_Monitoring / baseAnim_Monitoring_Time));
        yield return new WaitForSeconds(Wait_Monitoring);
        

        if (InvestigatedTargetHealthSetter)
        {
            if (!isEnemy1)
            {
                targetPoint = InvestigatedTargetHealthSetter.gameObject.transform.position;
                EvaluateRotation();
            }

            CurrentState = JamSpace.EState.ATTACKING;
        }
        else
        {
            CurrentState = JamSpace.EState.IDLE;
        }
    }

    public void StopFromSearch()
    {
        _canSearch = false;
        _counter = _waitAfterEndPoint;
    }

    void Update()
    {
        if (!_canSearch)
        {
            _counter -= Time.deltaTime;
            if (_counter <= 0)
            {
                _canSearch = true;
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
            if (isGroundCharging)
            {
                StopFromSearch();
            }
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
            if (!isEnemy1)
            {
                targetPoint = GetRandomPosititonBetweenPoints(ref point1, ref point2);
                EvaluateRotation();
            }
        }

        moveSpeed = normalSpeed;

        if (!isGroundCharging)
        {
            if (currentEState == JamSpace.EState.ATTACKING)
            {
                CurrentState = JamSpace.EState.COOLDOWN;
            }
        }
        else
        {
            CurrentState = JamSpace.EState.IDLE;
        }
    }

    [SerializeField]
    private Transform point1;
    // transform pt2 on right
    [SerializeField]
    private Transform point2;
    [SerializeField]
    public Vector2 targetPoint;

}
