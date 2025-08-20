using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolAI : MonoBehaviour
{
    public Transform[] patrolPoints;

    public float detectionRadius = 30f;
    public float chaseRadius = 40f;
    public float orbitRadius = 15f;
    public float orbitSpeed = 5f;

    private NavMeshAgent agent;
    [SerializeField] private Transform player;
    private EnemyWeaponSystem weaponSystem;

    private int currentPoint = 0;
    private enum State { Patrolling, Chasing, Orbiting }
    private State currentState = State.Patrolling;

    public void Initialize(Transform[] points, Transform playerTarget)
    {
        patrolPoints = points;
        player = playerTarget;
        weaponSystem = GetComponent<EnemyWeaponSystem>();
        if (weaponSystem != null)
            weaponSystem.SetTarget(player);

        agent = GetComponent<NavMeshAgent>();

        if (patrolPoints != null && patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[0].position);
    }

    void Update()
    {
        if (patrolPoints == null || patrolPoints.Length == 0 || player == null)
            return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrolling:
                Patrol();
                if (distanceToPlayer <= detectionRadius)
                    currentState = State.Chasing;
                break;

            case State.Chasing:
                agent.SetDestination(player.position);
                if (distanceToPlayer <= orbitRadius)
                    currentState = State.Orbiting;
                else if (distanceToPlayer > chaseRadius)
                    currentState = State.Patrolling;
                break;

            case State.Orbiting:
                OrbitAroundPlayer();
                if (distanceToPlayer > orbitRadius + 5f)
                    currentState = State.Chasing;
                break;
        }
    }

    void Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            currentPoint = (currentPoint + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[currentPoint].position);
        }
    }

    void OrbitAroundPlayer()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 orbitPos = player.position + Quaternion.Euler(0, 90, 0) * dir * orbitRadius;
        agent.SetDestination(orbitPos);
    }
}
