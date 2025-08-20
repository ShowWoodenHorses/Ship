using System.Collections;
using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Player
{
    public class ShipHealth : MonoBehaviour, IDamagable
    {
        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;

        private void Start()
        {
            currentHealth = maxHealth;
        }
        public void TakeDamage(int damage)
        {
            currentHealth -= damage;
            if (currentHealth < 0)
            {
                currentHealth = 0;
                Debug.Log("===== PLAYER DIE =========");
            }
        }

        public void SetHealth(int maxHealth)
        {
            this.maxHealth = maxHealth;
            currentHealth = maxHealth;
        }

    }
}