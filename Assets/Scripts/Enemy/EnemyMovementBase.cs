using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Базовый класс для всех типов передвижения врагов.
/// Содержит общие методы перемещения и стрельбы.
/// </summary>
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyWeaponSystem))]
public abstract class EnemyMovementBase : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected EnemyWeaponSystem weaponSystem;
    protected Transform target;

    [Header("Общие настройки движения")]
    public float moveSpeed = 8f;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        weaponSystem = GetComponent<EnemyWeaponSystem>();
        agent.speed = moveSpeed;
    }

    protected virtual void Start()
    {
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) SetTarget(player.transform);
        }
    }

    public virtual void SetTarget(Transform newTarget)
    {
        target = newTarget;
        weaponSystem?.SetTarget(newTarget);
    }

    protected void MoveTo(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
    }

    protected void Stop()
    {
        agent.isStopped = true;
    }

    protected bool IsTargetInRadius(float radius)
    {
        if (target == null) return false;
        return Vector3.Distance(transform.position, target.position) <= radius;
    }

    protected abstract void HandleMovement();

    protected virtual void Update()
    {
        if (target == null) return;

        HandleMovement();
    }
}
