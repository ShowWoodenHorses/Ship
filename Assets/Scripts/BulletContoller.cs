using System.Collections;
using Assets.Scripts.Interface;
using Assets.Scripts.ObjectPool;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletContoller : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private int damage;
        [SerializeField] private float lifeBeforeDestroy;

        [SerializeField] private GameObject effectShotInWater;
        [SerializeField] private GameObject effectShotInBuilding;

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void Initialize(Vector3 pos)
        {
            rb.linearVelocity = pos * speed;

            StartCoroutine(LifeBeforeDestroy());
        }
        public void InitializeWithTimer(Vector3 pos, float distance)
        {
            float time = distance / speed;
            lifeBeforeDestroy = time;
            rb.linearVelocity = pos * speed;

            StartCoroutine(LifeBeforeDestroy());
        }

        private void Deactive()
        {
            BulletObjectPool.Instance.ReturnObject(gameObject);
        }

        IEnumerator LifeBeforeDestroy()
        {
            yield return new WaitForSeconds(lifeBeforeDestroy);
            SpawnEffect(effectShotInWater);
            Deactive();
        }

        private void OnTriggerEnter(Collider other)
        {
            var objectForDamage = other.gameObject.GetComponent<IDamagable>();
            if (objectForDamage != null)
            {
                objectForDamage.TakeDamage(damage);
                Deactive();
            }
            var building = other.gameObject.GetComponent<IObstaclable>();
            if (building != null)
            {
                SpawnEffect(effectShotInBuilding);
                Deactive();
            }
        }

        private void SpawnEffect(GameObject effectObj)
        {
            GameObject effect = EffectObjectPool.Instance.GetObject(effectObj);
            effect.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
            EffectController effectController = effect.GetComponent<EffectController>();
            if (effectController != null)
            {
                effectController.Initialize(effect);
            }
        }

        public void SetSpeed(float s)
        {
            speed = s;
        }

        public void SetDamage(int dmg)
        {
            damage = dmg;
        }

        public void SetLifeTime(float time)
        {
            lifeBeforeDestroy = time;
        }
    }
}