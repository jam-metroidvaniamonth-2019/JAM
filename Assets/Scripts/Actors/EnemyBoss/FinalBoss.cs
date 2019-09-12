using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
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
    private void TriggerAbility(EAttackType _attackType)
    {
        var _triggerAbility = Array.Find(CollectionOfAttachedAbilities, element => element.attackType == _attackType);
        Vector2 _direction = Vector2.zero;

        if (_triggerAbility)
        {
            currentBossAbility = _triggerAbility;
            _triggerAbility.Trigger(_direction);
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

    private void Start()
    {
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
   