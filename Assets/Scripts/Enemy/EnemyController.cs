using UnityEngine;
using System;
using Assets.Scripts.Interface;

public class EnemyController : MonoBehaviour, IDamagable, IReward
{
    // Ссылка на префаб, от которого был создан враг
    [HideInInspector]
    public GameObject prefabRef;

    // Событие смерти врага
    public event Action<GameObject> OnEnemyDeath;

    private bool isDead = false;

    // Пример здоровья (можно заменить своей системой)
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int reward;
    [SerializeField] private bool isReward = true;

    private void OnEnable()
    {
        // Когда объект берётся из пула — восстанавливаем здоровье
        currentHealth = maxHealth;
        isDead = false;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            Die(true);
        }
    }

    public void Die(bool haveReward)
    {
        if (isDead) return;

        isDead = true;
        isReward = haveReward;

        // Вызываем событие смерти
        OnEnemyDeath?.Invoke(gameObject);

        // Важно: сам объект не уничтожаем, он вернётся в пул из EnemySpawner
        gameObject.SetActive(false);
    }

    public int GetReward()
    {
        return reward;
    }

    public bool IsReward()
    {
        return isReward;
    }
}
