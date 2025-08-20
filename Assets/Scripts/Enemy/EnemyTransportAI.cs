// EnemyTransportAI.cs
using UnityEngine;
using UnityEngine.AI;

public class EnemyTransportAI : MonoBehaviour
{
    public float detectionRadius = 25f;
    public float destanationDestroyBeforePointB = 2f;

    [SerializeField] private Transform player;
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    private NavMeshAgent agent;
    private EnemyWeaponSystem weaponSystem;

    public void Initialize(
        Transform playerTransform, 
        Transform pointA,
        Transform pointB)
    {
        player = playerTransform;
        this.pointA = pointA;
        this.pointB = pointB;
        agent = GetComponent<NavMeshAgent>();
        weaponSystem = GetComponent<EnemyWeaponSystem>();

        transform.position = pointA.position;
        agent.SetDestination(pointB.position);

        weaponSystem.SetTarget(player);
    }

    private void Update()
    {
        if(Vector3.Distance(pointB.position, transform.position) < destanationDestroyBeforePointB)
        {
            var enemyController = GetComponent<EnemyController>();
            if(enemyController != null)
            {
                enemyController.Die(false);
            }
        }
    }
}
