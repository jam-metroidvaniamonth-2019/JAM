using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JamSpace;

public class FlyDash : MonoBehaviour
{
    [SerializeField]
    float myBiteDamage;
    [SerializeField]
    float myTouchDamage;

    [SerializeField]
    DamagingComponent dmgComponent;
    [SerializeField]
    InvestigatingComponent investigatingComponent;

    

    [SerializeField]
    Animator myAnimator;

    [Header("Animation events time stamps")]
    [SerializeField]
    float investigationTimeLength;
    [SerializeField]
    float biteTimeLength;
    [SerializeField]
    float cooldownBiteTimeLength;
    [SerializeField]
    float cooldownLeapingTImeLength;
    [SerializeField]
    float constantChargingTime;

    public bool IsEnemyVisible
    {
        get
        {
            return _isEnemyVisible;
        }

        set
        {
            _isEnemyVisible = value;
            if (value)
            {
                if(currentState == EEnemyState.Idle)
                {
                    currentState = EEnemyState.Investigation;
                }
            }
        }
    }
    [SerializeField]
    bool _isEnemyVisible;

    public EEnemyState currentState
    {
        get
        {
            return _currentEnemyState;
        }
        set
        {
            _currentEnemyState = value;
            ExecuteState(value);

        }
    }
    [SerializeField]
    EEnemyState _currentEnemyState;

    [SerializeField]
    Vector2 offsetFromTargetPlayerPosition;
    [SerializeField]
    Vector2 targetChargingPoint;
    IEnumerator CallInvestigate(float _delayBeforeAction)
    {
        yield return new WaitForSeconds(_delayBeforeAction);
        targetChargingPoint = investigatingComponent.InvestigatedTargetHealthSetter.gameObject.transform.position;
        currentState = EEnemyState.Charging;
    }
    IEnumerator CallCharging(float _delayBeforeAction, Vector2 _targetPoint)
    {
        this.transform.position = _targetPoint - offsetFromTargetPlayerPosition;
        // make player charge in certain direction according to the received point 
        yield return new WaitForSeconds(_delayBeforeAction);
        if (dmgComponent.playerHealthSetter)
        {
            currentState = EEnemyState.Bite;
        }
        else
        {
            currentState = EEnemyState.Investigation;
        }
    }
    IEnumerator CallBite(float _delayBeforeAction)
    {
        yield return new WaitForSeconds(_delayBeforeAction);
        if (dmgComponent.DealDamage(myBiteDamage))
        {
            currentState = EEnemyState.Cooldown_B;
        }
        else {
            currentState = EEnemyState.Investigation;
        }
    }
    IEnumerator CallCooldownBite(float _delayBeforeAction)
    {
        yield return new WaitForSeconds(_delayBeforeAction);
        if (dmgComponent.playerHealthSetter)
        {
            currentState = EEnemyState.Bite;
        }
        else
        {
            currentState = EEnemyState.Investigation;
        }
    }

    void ExecuteState(EEnemyState _enemyState)
    {
        switch (_enemyState)
        {
            case EEnemyState.NONE:
                break;

            case EEnemyState.Idle:
                //myAnimator.SetTrigger(ANIMATION_IDLE);
                break;

            case EEnemyState.Investigation:
                //myAnimator.SetTrigger(ANIMATION_INVESTIGATE);
                StartCoroutine(CallInvestigate(investigationTimeLength));
                break;

            case EEnemyState.Charging:
                //myAnimator.SetTrigger(ANIMATION_CHARGE);
                StartCoroutine(CallCharging(constantChargingTime, targetChargingPoint));
                break;

            case EEnemyState.Bite:
                //myAnimator.SetTrigger(ANIMATION_BITE);
                StartCoroutine(CallBite(biteTimeLength));
                break;

            case EEnemyState.Cooldown_B:
                //myAnimator.SetTrigger(ANIMATION_COOLDOWN_BITE);
                StartCoroutine(CallCooldownBite(cooldownBiteTimeLength));
                break;

            case EEnemyState.MAX:
                break;
            default:
                break;
        }
    }
}
