using System.Collections;
using Assets.Scripts.Animation;
using Assets.Scripts.Game;
using Assets.Scripts.ObjectPool;
using Assets.Scripts.Player;
using Assets.Scripts.Save;
using Assets.Scripts.UI;
using Assets.Scripts.UI.Shop;
using DG.Tweening.Core.Easing;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        private SaveData data;

        [Header("Shop")]
        [SerializeField] private ShopBulletController shopBulletController;
        [SerializeField] private ShopShipController shopShipController;
        [SerializeField] private ScoreManager scoreManager;

        [Header("Pool")]
        [SerializeField] private EnemyObjectPool enemyPool;
        [SerializeField] private BulletObjectPool bulletPool;
        [SerializeField] private EffectObjectPool effectPool;

        [Header("Player")]
        [SerializeField] private ShipManager shipManager;
        [SerializeField] private ShipMovement shipMovement;
        [SerializeField] private Transform playerTransform;

        [Header("Spawner")]
        [SerializeField] private EnemySpawner enemySpawner;

        [Header("Generation")]
        [SerializeField] private MapGeneration mapGeneration;

        [Header("Save")]
        [SerializeField] private SaveLifecycle saveLifecycle;

        [Header("Animation")]
        [SerializeField] private GameplayAnimationController gameplayAnimationController;

        [Header("Game")]
        [SerializeField] private GameManager gameManager;
        [SerializeField] private UIController uiController;


        private void Awake()
        {
            data = SaveSystem.Load();

            saveLifecycle.Initialize(data);

            mapGeneration.Initialize();

            enemyPool.Initialize();
            bulletPool.Initialize();
            effectPool.Initialize();

            shopBulletController.Initialize(data.ownedItems, data.selectedBulletId, saveLifecycle);
            shopShipController.Initialize(data.ownedItems, data.selectedShipId, saveLifecycle);
            scoreManager.Initialize(data.currentCoins, data.allCoins, saveLifecycle);

            shipManager.Initialize(data.selectedShipId, data.selectedBulletId, gameplayAnimationController);
            gameManager.Initialize(uiController, scoreManager);
            enemySpawner.Initialize(data.currentWaveEnemyId, playerTransform);
        }
    }
}