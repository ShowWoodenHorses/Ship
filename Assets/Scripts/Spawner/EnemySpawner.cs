using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Configs;
using Assets.Scripts.Spawner;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("���� ���������")]
    public List<SpawnerBase> spawners;

    [Header("��������� ������")]
    public List<EnemyType> enemyTypes;
    public Transform playerTransform;
    public int maxTotalEnemies = 10;
    public float checkInterval = 5f;
    public float timeForUpdate = 10f;

    [SerializeField] private int currentTotalEnemies = 0;
    private float timer;

    [Header("Score Manager")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private EnemyWaveDatabase enemyWaveDatabase;
    [SerializeField] private string currentEnemyWaveId;

    [Header("��� ������")]
    public List<EnemyType> availableEnemies;

    private void OnEnable()
    {
        scoreManager.OnUpdateWave += OnUpdateSettingSpawn;
    }

    private void OnDisable()
    {
        scoreManager.OnUpdateWave -= OnUpdateSettingSpawn;
    }

    private void Start()
    {
        if (playerTransform == null)
        {
            var playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null) playerTransform = playerObj.transform;
        }

        if (playerTransform == null)
        {
            Debug.LogError("EnemySpawner: ����� �� ������!");
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

        availableEnemies = new List<EnemyType>();

        foreach (EnemyType enemy in enemyTypes)
        {
            if (enemy.currentCount < enemy.maxCount)
            {
                availableEnemies.Add(enemy);
            }
        }

        if (availableEnemies.Count == 0) return;

        EnemyType enemyToSpawn = availableEnemies[Random.Range(0, availableEnemies.Count)];

        Debug.Log("����� ������: " +  enemyToSpawn);

        foreach(var spawner in spawners)
        {
            if (spawner.HaveThisPrefab(enemyToSpawn.prefab))
            {
                GameObject enemyInstance = spawner.Spawn(playerTransform, enemyToSpawn.prefab);
                if (enemyInstance != null)
                {
                    enemyToSpawn.currentCount++;
                    currentTotalEnemies++;

                    // ��������� ���������� �������, ����� ����������� ����������� �����
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

    private void OnEnemyDeath(GameObject enemy)
    {
        // ��������� ��������
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

        // ���������� ������ � ���
        EnemyObjectPool.Instance.ReturnObject(enemy);
    }

    private void OnUpdateSettingSpawn(string enemyWaveId)
    {
        EnemyWaveConfig config = enemyWaveDatabase.GetEnemyConfigById(enemyWaveId);

        if (config == null) return;

        if (currentEnemyWaveId == config.id) return;

        checkInterval = timeForUpdate;
        currentEnemyWaveId = config.id;
        UpdateEnemyTypes(config.enemyTypes);
        UpdateMaxCountEnemy(config.maxLimitEnemy);
        UpdateCheckInterval(config.intervalSpawn);

        Debug.Log($"����� ������: {config.id}");
    }

    private void UpdateEnemyTypes(EnemyType[] newEnemyTypes)
    {
        enemyTypes.Clear();
        foreach(var newEnemy in newEnemyTypes)
        {
            enemyTypes.Add(newEnemy);
            newEnemy.currentCount = 0;
        }
    }

    private void UpdateMaxCountEnemy(int newMaxLimitEnemy)
    {
        maxTotalEnemies = newMaxLimitEnemy;
    }

    private void UpdateCheckInterval(int newInterval)
    {
        checkInterval = newInterval;
    }
}