using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Spawner;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public int maxCount;
        [HideInInspector]
        public int currentCount;
    }

    [Header("Виды спавнеров")]
    public List<SpawnerBase> spawners;

    [Header("Настройки спавна")]
    public List<EnemyType> enemyTypes;
    public Transform playerTransform;
    public int maxTotalEnemies = 10;
    public float checkInterval = 5f;

    [SerializeField] private int currentTotalEnemies = 0;
    private float timer;

    [Header("Score Manager")]
    [SerializeField] private ScoreManager scoreManager;

    private void Start()
    {
        if (playerTransform == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }

        if (playerTransform == null)
        {
            Debug.LogError("EnemySpawner: Игрок не найден!");
            enabled = false;
            return;
        }

        timer = checkInterval;
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            CheckAndSpawn();
            timer = checkInterval;
        }
    }

    protected virtual void CheckAndSpawn()
    {
        if (currentTotalEnemies >= maxTotalEnemies) return;

        List<EnemyType> availableEnemies = new List<EnemyType>();

        foreach (EnemyType enemy in enemyTypes)
        {
            if (enemy.currentCount < enemy.maxCount)
            {
                availableEnemies.Add(enemy);
            }
        }

        if (availableEnemies.Count == 0) return;

        EnemyType enemyToSpawn = availableEnemies[Random.Range(0, availableEnemies.Count)];

        foreach(var spawner in spawners)
        {
            if (spawner.HaveThisPrefab(enemyToSpawn.prefab))
            {
                GameObject enemyInstance = spawner.Spawn(playerTransform, enemyToSpawn.prefab);
                if (enemyInstance != null)
                {
                    enemyToSpawn.currentCount++;
                    currentTotalEnemies++;

                    // Добавляем обработчик события, чтобы отслеживать уничтожение врага
                    var enemyController = enemyInstance.GetComponent<EnemyController>();
                    if (enemyController != null)
                    {
                        enemyController.prefabRef = enemyToSpawn.prefab;
                        enemyController.OnEnemyDeath += OnEnemyDeath;
                    }
                }
            }
        }
    }

    protected void OnEnemyDeath(GameObject enemy)
    {
        // Уменьшаем счетчики
        currentTotalEnemies--;

        var enemyController = enemy.GetComponent<EnemyController>();
        foreach (EnemyType enemyType in enemyTypes)
        {
            if (enemyType.prefab == enemyController.prefabRef)
            {
                enemyType.currentCount--;
                if (enemyController.IsReward())
                {
                    scoreManager.AddScore(enemyController.GetReward());
                }
                break;
            }
        }
        enemyController.OnEnemyDeath -= OnEnemyDeath;

        // Возвращаем объект в пул
        EnemyObjectPool.Instance.ReturnObject(enemy);
    }
}