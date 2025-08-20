using Assets.Scripts.Configs;
using UnityEngine;

namespace Assets.Scripts.Player
{
    using UnityEngine;

    public class ShipManager : MonoBehaviour
    {
        [Header("Корабль")]
        [SerializeField] private ShipDatabase shipDatabase;
        [SerializeField] private string startShipId = "sloop"; // стартовый корабль
        [Header("Снаряд")]
        [SerializeField] private BulletDatabase bulletDatabase;
        [SerializeField] private string startBulletPrefabId = "bullet1";
        [SerializeField] private GameObject currentBulletPrefab;

        private GameObject currentShipInstance;
        private ShipMovement movement;
        private ShipHealth health;
        private ShipCannonMultiSide cannons;

        void Start()
        {
            UpgradeShip(startShipId);
            UpgradeBullet(startBulletPrefabId);
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
            // удалить старый
            if (currentShipInstance != null)
                Destroy(currentShipInstance);

            // создать новый
            currentShipInstance = Instantiate(config.shipPrefab, transform.position, transform.rotation, transform);

            // подключить компоненты
            movement = GetComponent<ShipMovement>();
            health = currentShipInstance.GetComponent<ShipHealth>();
            cannons = currentShipInstance.GetComponent<ShipCannonMultiSide>();

            // применить параметры
            if (movement != null)
            {
                movement.acceleration = config.acceleration;
                movement.maxSpeed = config.maxSpeed;
                movement.deceleration = config.deceleration;
                movement.turnSpeed = config.turnSpeed;
            }

            if (health != null)
            {
                health.SetHealth(config.maxHealth);
            }
        }

        private void UpdateBullet(BulletConfig config)
        {
            currentBulletPrefab = config.bulletPrefab;
            BulletContoller bulletController = currentBulletPrefab.GetComponent<BulletContoller>();
            if(bulletController != null)
            {
                bulletController.SetSpeed(config.speed);
                bulletController.SetDamage(config.damageEnemy);
                bulletController.SetLifeTime(config.lifeBeforeDestroy);
            }

            ShipCannonMultiSide shipCannonMultiSide = transform.GetChild(0).GetComponent<ShipCannonMultiSide>();
            if(shipCannonMultiSide != null)
            {
                shipCannonMultiSide.InitializeBullet(currentBulletPrefab);
            }
        }
    }

}