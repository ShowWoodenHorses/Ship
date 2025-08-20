using System.Collections;
using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class PatrolSpawner : SpawnerBase
    {
        public GameObject patrolEnemyPrefab;
        public Transform[] patrolRoutePoints; // точки на карте
        public int count = 3;

        public override bool HaveThisPrefab(GameObject prefab)
        {
            if (patrolEnemyPrefab == prefab)
            {
                return true;
            }
            return false;
        }

        public override GameObject Spawn(Transform playerTransform, GameObject patrolEnemyPrefab)
        {
            GameObject enemy = EnemyObjectPool.Instance.GetObject(patrolEnemyPrefab);

            if (enemy == null) return null;

            EnemyPatrolAI patrolAI = enemy.GetComponent<EnemyPatrolAI>();
            if (patrolAI != null)
            {
                patrolAI.Initialize(patrolRoutePoints, playerTransform);
            }

            return enemy;
        }
    }
}