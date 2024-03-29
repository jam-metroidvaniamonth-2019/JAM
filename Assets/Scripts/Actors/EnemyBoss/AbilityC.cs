﻿using System;
using System.Collections;
using UnityEngine;
using Utils;
using WorldDisplay;

public class AbilityC : BaseBossAbility
{
    [SerializeField] private GameObject[] _stoneFallingHolderPrefab;
    [SerializeField] private GameObject _stoneSpawnEffect;
    [SerializeField] private float _stoneFallingWaitTime = 1;

    private Transform _stonesFallingPoint;
    private MakeChildrenFall _stoneFallingActivator;

    public override void Trigger(Vector2 _direction)
    {
        base.Trigger(_direction);
        NotifyAbilityCompleted();

        LaunchAttack();
    }

    #region Unity Functions

    private void Start()
    {
        _stonesFallingPoint = GameObject.FindGameObjectWithTag(TagManager.FinalBossStoneFallingPoint).transform;
    }

    #endregion

    #region Utility Functions

    private void LaunchAttack()
    {
        GameObject stoneFallingInstance =
            Instantiate(_stoneFallingHolderPrefab[UnityEngine.Random.Range(0,_stoneFallingHolderPrefab.Length)], _stonesFallingPoint.position, Quaternion.identity);
        _stoneFallingActivator = stoneFallingInstance.GetComponent<MakeChildrenFall>();

        Instantiate(_stoneSpawnEffect, _stonesFallingPoint.position, Quaternion.identity);
        StartCoroutine(ActivateStoneFalling());
    }

    private IEnumerator ActivateStoneFalling()
    {
        yield return new WaitForSeconds(_stoneFallingWaitTime);
        _stoneFallingActivator.ThrowChildrenWithForce();
    }

    #endregion
}