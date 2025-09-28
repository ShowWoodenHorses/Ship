using Assets.Scripts.Configs;
using Assets.Scripts.Animation;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player
{
    public class ShipManager : MonoBehaviour
    {
        [Header("Корабль")]
        [SerializeField] private ShipDatabase shipDatabase;
        [Header("Снаряд")]
        [SerializeField] private BulletDatabase bulletDatabase;
        [SerializeField] private string currentBulletPrefabId;
        [SerializeField] private GameObject currentBulletPrefab;
        [SerializeField] private Slider healthBarSlider;

        private GameObject currentShipInstance;
        private ShipMovement movement;
        private ShipHealth health;
        private ShipCannonMultiSide cannons;
        private GameplayAnimationController gameplayAnimationController;
        private SailController sailController;
        private ShipWakeParticles shipWakeParticles;

        //Bootstrap
        //void Start()
        //{
        //    currentBulletPrefabId = startBulletPrefabId;
        //    UpgradeShip(startShipId);
        //}

        public void Initialize(string shipId, string bulletId, GameplayAnimationController gameplayAnimationController)
        {
            this.gameplayAnimationController = gameplayAnimationController;
            currentBulletPrefabId = bulletId;
            UpgradeShip(shipId);
        }

        public void UpgradeShip(string shipId)
        {
            ShipConfig config = shipDatabase.GetShipById(shipId);
            if (config == null)
            {
                Debug.LogError($"Ship with id '{shipId}' not found in database!");
                return;
            }

            SpawnShip(config);
        }

        public void UpgradeBullet(string bulletId)
        {
            BulletConfig bulletConfig = bulletDatabase.GetBulletById(bulletId);
            if(bulletConfig == null)
            {
                Debug.LogWarning($"{bulletId} не существует!");
            }
            UpdateBullet(bulletConfig);
        }

        public GameObject GetCurrentBulletPrefab()
        {
            return currentBulletPrefab;
        }

        private void SpawnShip(ShipConfig config)
        {
            if (currentShipInstance != null)
            {
                gameplayAnimationController.DeleteAnimations(currentShipInstance);
                Destroy(currentShipInstance);
            }

            currentShipInstance = Instantiate(config.shipPrefab, transform.position, transform.rotation, transform);

            movement = GetComponent<ShipMovement>();
            health = currentShipInstance.GetComponent<ShipHealth>();
            cannons = currentShipInstance.GetComponent<ShipCannonMultiSide>();
            sailController = currentShipInstance.GetComponent<SailController>();
            shipWakeParticles = currentShipInstance.GetComponent<ShipWakeParticles>();

            cannons.Initialize(gameplayAnimationController);

            gameplayAnimationController.ShipSway(currentShipInstance.transform, 5f, 2f);
            gameplayAnimationController.LowerSails(sailController.sailDown, sailController.sailUp, sailController.transitionTime);

            if (movement != null)
            {
                movement.Initialize(
                    config.acceleration,
                    config.maxSpeed, 
                    config.deceleration, 
                    config.turnSpeed
                    );
            }

            if (health != null)
            {
                health.Initialize(config.maxHealth, healthBarSlider);
                healthBarSlider.maxValue = config.maxHealth;
                healthBarSlider.value = config.maxHealth;
            }

            shipWakeParticles.Initialize(movement);

            UpgradeBullet(currentBulletPrefabId);
        }

        private void UpdateBullet(BulletConfig config)
        {
            currentBulletPrefab = config.bulletPrefab;
            BulletContoller bulletController = currentBulletPrefab.GetComponent<BulletContoller>();
            if(bulletController != null)
            {
                bulletController.SetSpeed(config.speed);
                bulletController.SetDamageEnemy(config.damageEnemy);
                bulletController.SetDamageBuilding(config.damageBuilding);
                bulletController.SetLifeTime(config.lifeBeforeDestroy);
            }

            ShipCannonMultiSide shipCannonMultiSide = transform.GetChild(0).GetComponent<ShipCannonMultiSide>();
            if(shipCannonMultiSide != null)
            {
                shipCannonMultiSide.UpdateBullet(currentBulletPrefab);
            }
            currentBulletPrefabId = config.id;
        }
    }

}