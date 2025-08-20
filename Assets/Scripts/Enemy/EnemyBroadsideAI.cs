// EnemyBroadsideAI.cs (упрощённая версия)
using UnityEngine;
using UnityEngine.AI;

public class EnemyBroadsideAI : EnemyMovementBase
{
    [Header("Орбита вокруг игрока")]
    public float orbitRadius = 22f;
    public int orbitPointsCount = 24;
    public float waypointAdvanceDistance = 5f;
    public float enterOrbitDistance = 28f;
    public float exitOrbitDistance = 40f;
    public float turnRateDegPerSec = 180f;

    private enum EnemyState { Pursuing, Orbiting }
    private EnemyState state = EnemyState.Pursuing;

    private Vector3[] orbitWaypoints;
    private int currentWPIndex;

    protected override void Start()
    {
        base.Start();
        orbitWaypoints = new Vector3[orbitPointsCount];
        UpdateOrbitWaypoints();
    }

    protected override void HandleMovement()
    {
        if (!target) return;

        float dist = Vector3.Distance(transform.position, target.position);
        UpdateOrbitWaypoints();

        switch (state)
        {
            case EnemyState.Pursuing:
                agent.SetDestination(target.position);
                RotateTowardsAgentVelocity();
                if (dist <= enterOrbitDistance) state = EnemyState.Orbiting;
                break;

            case EnemyState.Orbiting:
                OrbitMovement();
                if (dist >= exitOrbitDistance) state = EnemyState.Pursuing;
                break;
        }
    }

    void OrbitMovement()
    {
        Vector3 wp = orbitWaypoints[currentWPIndex];
        agent.SetDestination(wp);

        if (Vector3.Distance(transform.position, wp) < waypointAdvanceDistance)
            currentWPIndex = (currentWPIndex + 1) % orbitWaypoints.Length;

        Vector3 moveDir = (wp - transform.position).normalized;
        if (moveDir.sqrMagnitude > 0.01f)
        {
            Quaternion desiredRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, turnRateDegPerSec * Time.deltaTime);
        }
    }

    void RotateTowardsAgentVelocity()
    {
        Vector3 vel = agent.desiredVelocity;
        if (vel.sqrMagnitude > 0.01f)
        {
            Quaternion desiredRot = Quaternion.LookRotation(vel.normalized, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, turnRateDegPerSec * Time.deltaTime);
        }
    }

    void UpdateOrbitWaypoints()
    {
        if (!target) return;
        for (int i = 0; i < orbitPointsCount; i++)
        {
            float angle = (360f / orbitPointsCount) * i;
            Vector3 offset = Quaternion.Euler(0, angle, 0) * Vector3.forward * orbitRadius;
            orbitWaypoints[i] = target.position + offset;
        }
    }
}
