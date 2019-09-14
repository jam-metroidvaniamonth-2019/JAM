using System;
using System.Collections;
using System.Collections.Generic;
using CustomCamera;
using UI.CutScene;
using UnityEngine;

public class FinalBoss : BaseNPC
{
    public Animator myAnimator;

    public const string ANIMATION_IDLE = "ANIMATION_IDLE";
    public const string ANIMATION_BITE = "ANIMATION_BITE";
    public const string ANIMATION_HIT = "ANIMATION_HIT";

    private void CallAnimator(string _animTag)=> myAnimator.SetTrigger(_animTag);

    [Header("CutSceneAfterDeath")]
    public Sprite cutSceneImg;
    public float cutSceneDuration;
    public Vector3 cameraOffset;
    public CameraController cameraController;

    public Transform projectileAFiringPt;
    public Transform projectileBFiringPt;

    public AbilityD abilityD;

    public void StartBossBattle()
    {
        CallNextAbility();
    }
    public BaseBossAbility currentBossAbility;

    [SerializeField]
    public Player.Movement.PlayerCollision playerCol;

    [SerializeField]
    private List<EAttackType> attackSequence;
    [SerializeField]
    private int counter = 0;

    [SerializeField]
    private BaseBossAbility[] CollectionOfAttachedAbilities;

    [Header("Wait Before Fire")]
    [SerializeField]
    private float _waitBeforeFire;

    private void CallNextAbility() => StartCoroutine(ArtificialWaitOnAttack(attackSequence[CycleAbilityCounter()],_waitBeforeFire));
    private int CycleAbilityCounter()
    {
        ++counter;
        return counter %= CollectionOfAttachedAbilities.Length;
    }

    private void CallSequencer()
    {
        CallNextAbility();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            abilityD.Trigger(Vector2.zero);
        }
    }


    private Vector2 GetDirecitonForAbilityAttack(EAttackType _attackType)
    {
        Vector3 _direction = new Vector3(0, 0, 0);

        if (_attackType == EAttackType.Ability_A)
        {
            _direction = (playerCol.transform.position - projectileAFiringPt.position).normalized;
            Debug.DrawRay(projectileAFiringPt.position, _direction,Color.red,3f);
            return _direction;

        }else if (_attackType == EAttackType.Ability_B)
        {
            _direction = (playerCol.transform.position - projectileBFiringPt.position).normalized;
            Debug.DrawRay(projectileBFiringPt.position, _direction, Color.red, 3f);
            return _direction;
        }
        else
        {
            return Vector2.zero;
        }
    }

    private IEnumerator ArtificialWaitOnAttack(EAttackType _attackType, float _wait)
    {
        CallAnimator(ANIMATION_BITE);
        yield return new WaitForSeconds(_wait);
        TriggerAbility(_attackType);
    }

    private void TriggerAbility(EAttackType _attackType)
    {
        var _triggerAbility = Array.Find(CollectionOfAttachedAbilities, element => element.attackType == _attackType);

        if (_triggerAbility)
        {
            currentBossAbility = _triggerAbility;
            _triggerAbility.Trigger(GetDirecitonForAbilityAttack(_attackType));
        }
    }
    private IEnumerator TriggerAbilityRoutine(float _wait)
    {
        yield return new WaitForSeconds(_wait);
        CallSequencer();
    }
    private void TriggerAbilityCooldown(float _abilityCooldownDuration)
    {
        StartCoroutine(TriggerAbilityRoutine(_abilityCooldownDuration));
    }

    public void DsiplayCutscene()
    {
        CutSceneDisplay.Instance.DisplayCutScene(cutSceneImg, cutSceneDuration);
    }

    private void CallHealthReduceAnimation(float _redVal1, float _redVal2) {
        CallAnimator(ANIMATION_HIT);
    }

    public override void Start()
    {
        base.Start();

        myAnimator = GetComponent<Animator>();

        CallAnimator(ANIMATION_IDLE);
        enemyHealthObject.enemyHealthSetter.OnHealthChanged += CallHealthReduceAnimation;

        enemyHealthObject.enemyHealthSetter.OnHealthZero += DsiplayCutscene;
        cameraController.UpdatePlayerOffset(cameraOffset);

        foreach (var element in CollectionOfAttachedAbilities)
        {
            element.OnAbilityCompleted += TriggerAbilityCooldown;
        }
    }
    private void OnDestroy()
    {
        foreach (var element in CollectionOfAttachedAbilities)
        {
            element.OnAbilityCompleted -= TriggerAbilityCooldown;
        }
    }
}
    public enum EAttackType
    {
        NONE,
        Ability_A,
        Ability_B,
        Ability_C,
        Ability_D,
        MAX
    }
   