using System.Collections;
using UnityEngine;
using WorldDisplay;

public class AbilityC : BaseBossAbility
{
    [SerializeField] private Transform _stonesFallingPoint;
    [SerializeField] private GameObject _stoneFallingHolderPrefab;
    [SerializeField] private GameObject _stoneSpawnEffect;
    [SerializeField] private float _stoneFallingWaitTime = 1;

    private MakeChildrenFall _stoneFallingActivator;

    public override void Trigger(Vector2 _direction)
    {
        base.Trigger(_direction);
        NotifyAbilityCompleted();

        LaunchAttack();
    }

    private void LaunchAttack()
    {
        GameObject stoneFallingInstance =
            Instantiate(_stoneFallingHolderPrefab, _stonesFallingPoint.position, Quaternion.identity);
        _stoneFallingActivator = stoneFallingInstance.GetComponent<MakeChildrenFall>();

        Instantiate(_stoneSpawnEffect, _stonesFallingPoint.position, Quaternion.identity);
    }

    private IEnumerator ActivateStoneFalling()
    {
        yield return new WaitForSeconds(_stoneFallingWaitTime);
        _stoneFallingActivator.ThrowChildrenWithForce();
    }
}