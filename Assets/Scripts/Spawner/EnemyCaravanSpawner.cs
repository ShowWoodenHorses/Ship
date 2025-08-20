
using UnityEngine;
using System.Collections;
using Assets.Scripts.Spawner;

public class EnemyCaravanSpawner : SpawnerBase
{
    public GameObject caravanPrefab;
    public GameObject[] enemyPrefabs;
    public Transform[] spawnPoints;
    public Transform[] destinationPoints;
    public float spawnInterval = 2f;

    IEnumerator SpawnCaravanGuards(Transform playerTransform, Transform pointA, Transform pointB)
    {
        if (spawnPoints.Length == 0 || destinationPoints.Length == 0 || enemyPrefabs.Length == 0)
            yield break;

        for (int i = 0; i < enemyPrefabs.Length; i++)
        {
            GameObject prefab = enemyPrefabs[i];
            GameObject enemy = EnemyObjectPool.Instance.GetObject(prefab);

            EnemyTransportAI transportAI = enemy.GetComponent<EnemyTransportAI>();

            if (transportAI != null)
            {
                transportAI.Initialize(playerTransform, pointA, pointB);
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public override GameObject Spawn(Transform playerTransform, GameObject prefab)
    {
        Transform pointA = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Transform pointB = destinationPoints[Random.Range(0, destinationPoints.Length)];

        GameObject caravanObj = EnemyObjectPool.Instance.GetObject(prefab);
        if (caravanObj != null)
        {
            EnemyTransportAI caravanTransportAI = caravanObj.GetComponent<EnemyTransportAI>();
            if(caravanTransportAI != null)
            {
                caravanTransportAI.Initialize(playerTransform, pointA, pointB);
                StartCoroutine(SpawnCaravanGuards(playerTransform, pointA, pointB));
                return caravanObj;
            }
        }

        return null;
    }

    public override bool HaveThisPrefab(GameObject prefab)
    {
        return caravanPrefab == prefab;
    }
}
