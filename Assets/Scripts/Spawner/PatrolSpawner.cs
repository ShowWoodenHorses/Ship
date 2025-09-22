using Assets.Scripts.Animation;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public class PatrolSpawner : SpawnerBase
    {
        public GameObject patrolEnemyPrefab;
        public Transform[] patrolRoutePoints; // точки на карте

        public override bool HaveThisPrefab(GameObject prefab)
        {
            if (patrolEnemyPrefab == prefab)
            {
                return true;
            }
            return false;
        }

        public override GameObject Spawn(Transform playerTransform, GameObject patrolEnemyPrefab, GameplayAnimationController gameplayAnimationController)
        {
            GameObject enemy = EnemyObjectPool.Instance.GetObject(patrolEnemyPrefab);

            if (enemy == null) return null;

            EnemyPatrolAI patrolAI = enemy.GetComponent<EnemyPatrolAI>();
            if (patrolAI != null)
            {
                patrolAI.Initialize(patrolRoutePoints, playerTransform);
            }

            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.Initialize(patrolEnemyPrefab, gameplayAnimationController);
            }

            return enemy;
        }
    }
}