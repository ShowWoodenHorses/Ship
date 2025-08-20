using UnityEngine;
using System;
using Assets.Scripts.Interface;

public class EnemyController : MonoBehaviour, IDamagable, IReward
{
    // ������ �� ������, �� �������� ��� ������ ����
    [HideInInspector]
    public GameObject prefabRef;

    // ������� ������ �����
    public event Action<GameObject> OnEnemyDeath;

    private bool isDead = false;

    // ������ �������� (����� �������� ����� ��������)
    public int maxHealth = 100;
    [SerializeField] private int currentHealth;
    [SerializeField] private int reward;
    [SerializeField] private bool isReward = true;

    private void OnEnable()
    {
        // ����� ������ ������ �� ���� � ��������������� ��������
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

        // �������� ������� ������
        OnEnemyDeath?.Invoke(gameObject);

        // �����: ��� ������ �� ����������, �� ������� � ��� �� EnemySpawner
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
