using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityD : BaseBossAbility
{
    public Transform[] verticesOfTheAreaInWhichToSpawnMinions;
    private const float minimuimDelayBeforeSpawn = 0.01f;
    [SerializeField]
    private List<GameObject> minionPrefabs;
    [SerializeField]
    private float delayBeforeEverySpawn;
    [SerializeField]
    private int numberOfMinonsToSpawn;

    private GameObject GetRandomMinionPrefab()
    {
        return minionPrefabs[Random.Range(0, minionPrefabs.Count)];
    }
    private Vector2 GetRandomPosition()
    {
        return new Vector2(0, 0);
    }
    public override void Trigger(Vector2 _direction)
    {
        base.Trigger(_direction);
        //StartCoroutine(spawnEnemies());
        NotifyAbilityCompleted();

    }
    private IEnumerator spawnEnemies()
    {
        delayBeforeEverySpawn = Mathf.Max(delayBeforeEverySpawn, minimuimDelayBeforeSpawn);
        yield return new WaitForSeconds(delayBeforeEverySpawn);
        --numberOfMinonsToSpawn;
        GameObject.Instantiate(GetRandomMinionPrefab(), GetRandomPosition(), Quaternion.identity);
        if (numberOfMinonsToSpawn > 0)
        {
            StartCoroutine(spawnEnemies());
        }
    }

}
