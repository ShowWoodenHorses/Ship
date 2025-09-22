using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Enemy
{
    public class StandCannon : MonoBehaviour
    {
        [SerializeField] private Transform target;

        [Header("setting")]
        [SerializeField] private float timeReload;
        [SerializeField] private float fireDistanceMax;
        [SerializeField] private float fireDistanceMin;
        [SerializeField] private GameObject prefabBullet;
        [SerializeField] private GameObject cannonObject;
        [SerializeField] private Transform positionBullet;

        private float timeCooldown;

        private void Start()
        {
            timeCooldown = timeReload;
        }

        private void Update()
        {
            if (target == null) return;
            HandleLogic();
        }

        public void Initialize(Transform playerTransform)
        {
            target = playerTransform;
            timeCooldown = timeReload;
        }

        private void HandleLogic()
        {
            timeCooldown -= Time.deltaTime;

            Vector3 direction = (target.position - cannonObject.transform.position).normalized;

            cannonObject.transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            float distance = Vector3.Distance(target.position, transform.position);

            if (distance <= fireDistanceMax && distance > fireDistanceMin)
            {
                if(timeCooldown <= 0f)
                {
                    Shoot(direction, distance);
                    timeCooldown = timeReload;
                }
            }
        }

        private void Shoot(Vector3 direction, float distance)
        {
            GameObject bullet = BulletObjectPool.Instance.GetObject(prefabBullet);
            bullet.transform.SetPositionAndRotation(positionBullet.position, Quaternion.LookRotation(direction, Vector3.up));

            BulletContoller bulletContoller = bullet.GetComponent<BulletContoller>();
            if(bulletContoller != null)
            {
                bulletContoller.InitializeWithTimer(cannonObject.transform.forward, distance);
            }
        }
    }
}