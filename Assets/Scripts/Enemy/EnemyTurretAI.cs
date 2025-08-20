using UnityEngine;

public class EnemyTurretAI : EnemyMovementBase
{
    [Header("Параметры поведения")]
    public float stopRadius = 20f;
    public float orbitSpeed = 5f;
    public bool clockwise = true;

    protected override void HandleMovement()
    {
        if (!target) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > stopRadius)
        {
            MoveTo(target.position);
        }
        else
        {
            OrbitAroundTarget();
        }
    }

    void OrbitAroundTarget()
    {
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        dirToTarget.y = 0;

        Vector3 tangent = clockwise
            ? Quaternion.Euler(0, 90f, 0) * dirToTarget
            : Quaternion.Euler(0, -90f, 0) * dirToTarget;

        Vector3 orbitTarget = transform.position + tangent * orbitSpeed;
        MoveTo(orbitTarget);
    }
}
