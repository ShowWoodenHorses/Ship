using System.Collections;
using Assets.Scripts.Interface;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class BulletContoller : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private int damage;
        [SerializeField] private float lifeBeforeDestroy;

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

        private void Deactive()
        {
            BulletObjectPool.Instance.ReturnObject(gameObject);
        }

        IEnumerator LifeBeforeDestroy()
        {
            yield return new WaitForSeconds(lifeBeforeDestroy);
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