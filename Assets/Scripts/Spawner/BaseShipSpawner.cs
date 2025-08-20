using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Spawner
{
    public class BaseShipSpawner : SpawnerBase
    {
        public List<GameObject> prefabs = new List<GameObject>();
        public List<Transform> spawnPoints;

        public override bool HaveThisPrefab(GameObject prefabToSpawn)
        {
            foreach (GameObject prefab in prefabs)
            {
                if (prefab == prefabToSpawn)
                {
                    return true;
                }
            }
            return false;
        }

        public override GameObject Spawn(Transform playerTransform, GameObject enemyToSpawn)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];

            if (NavMesh.SamplePosition(spawnPoint.position, out NavMeshHit hit, 5f, NavMesh.AllAreas))
            {
                GameObject enemy = EnemyObjectPool.Instance.GetObject(enemyToSpawn);
                if (enemy != null)
                {
                    enemy.transform.position = hit.position;
                    enemy.transform.rotation = Quaternion.identity;

                    var ai = enemy.GetComponent<EnemyMovementBase>();
                    if (ai != null)
                    {
                        ai.SetTarget(playerTransform);
                    }

                    return enemy;
                }
            }

            return null;
        }
    }
}