using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityD : BaseBossAbility
{
    public GameObject antidotePrefab;

    private void SpawnAntidote(Vector2 _position)
    {
        GameObject.Instantiate(antidotePrefab, _position, Quaternion.identity);
    }

    public Transform[] verticesOfTheAreaInWhichToSpawnMinions;
    private const float minimuimDelayBeforeSpawn = 0.01f;
    [SerializeField]
    private List<GameObject> minionPrefabs;
    [SerializeField]
    private float delayBeforeEverySpawn;
    [SerializeField]
    private int numberOfMinonsToSpawn;

    [SerializeField]
    public List<BaseNPC> allSpawnedNPCs;

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
        StartCoroutine(spawnEnemies());
        NotifyAbilityCompleted();

    }

    private void EnemyDiedAt(BaseNPC _enemyScript)
    {
        if (allSpawnedNPCs.Contains(_enemyScript))
        {
            allSpawnedNPCs.Remove(_enemyScript);
            if(allSpawnedNPCs.Count == 0)
            {
                // all dead spawn antidote here
                SpawnAntidote(_enemyScript.transform.position);
            }
        }
    }

    private IEnumerator spawnEnemies()
    {
        delayBeforeEverySpawn = Mathf.Max(delayBeforeEverySpawn, minimuimDelayBeforeSpawn);
        yield return new WaitForSeconds(delayBeforeEverySpawn);
        --numberOfMinonsToSpawn;
        var enemyMinion = GameObject.Instantiate(GetRandomMinionPrefab(), GetRandomPosition(), Quaternion.identity);
        var NPCScript = enemyMinion.GetComponent<BaseNPC>();
        NPCScript.OnEnemyDeathPosition += EnemyDiedAt;
        allSpawnedNPCs.Add(NPCScript);

        if (numberOfMinonsToSpawn > 0)
        {
            StartCoroutine(spawnEnemies());
        }
    }

}
