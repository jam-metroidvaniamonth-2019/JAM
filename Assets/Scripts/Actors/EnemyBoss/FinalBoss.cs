using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : BaseNPC
{
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
    private void CallNextAbility() => TriggerAbility(attackSequence[CycleAbilityCounter()]);
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
        if(_attackType == EAttackType.Ability_A)
        {
            var _direction = (playerCol.transform.localPosition - projectileAFiringPt.localPosition).normalized;
            Debug.DrawRay(_direction, projectileAFiringPt.localPosition, Color.gray,5f);
            return _direction;

        }else if (_attackType == EAttackType.Ability_B)
        {
            var _direction = (playerCol.transform.localPosition - projectileBFiringPt.position).normalized;
            Debug.DrawRay(_direction, projectileBFiringPt.position,Color.red,5f);
            return _direction;
        }
        else
        {
            return Vector2.zero;
        }
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
    public override void Start()
    {
        base.Start();

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
   