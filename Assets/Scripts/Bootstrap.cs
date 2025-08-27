using System.Collections;
using Assets.Scripts.Player;
using Assets.Scripts.Save;
using Assets.Scripts.UI.Shop;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [Header("Shop")]
        [SerializeField] private ShopBulletController shopBulletController;
        [SerializeField] private ShopShipController shopShipController;
        [SerializeField] private ScoreManager scoreManager;

        [Header("Pool")]
        [SerializeField] private EnemyObjectPool enemyPool;
        [SerializeField] private BulletObjectPool bulletPool;

        [Header("Player")]
        [SerializeField] private ShipManager shipManager;
        [SerializeField] private ShipMovement shipMovement;

        [Header("Spawner")]
        [SerializeField] private EnemySpawner enemySpawner;

        [Header("Generation")]
        [SerializeField] private MapGeneration mapGeneration;


        private void Awake()
        {
            SaveData data = SaveSystem.Load();

            mapGeneration.Initialize();

            enemyPool.Initialize();
            bulletPool.Initialize();

            shopBulletController.Initialize(data.ownedItems, data.selectedBulletId);
            shopShipController.Initialize(data.ownedItems, data.selectedShipId);
            scoreManager.Initialize(data.currentCoins, data.allCoins);

            shipManager.Initialize(data.selectedShipId, data.selectedBulletId);
            enemySpawner.Initialize(data.currentWaveEnemyId);
        }
    }
}