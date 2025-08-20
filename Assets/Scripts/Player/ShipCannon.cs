using UnityEngine;

public class ShipCannon : MonoBehaviour
{
    public ShipCannonMultiSide.CannonSide side = ShipCannonMultiSide.CannonSide.Front;

    [Header("ѕараметры поворота")]
    public float maxLeftRotation = 45f;
    public float maxRightRotation = 45f;
    public float rotationSpeed = 5f;
    public Transform ShotPos;

    private ShipCannonMultiSide manager;
    private Vector3 sideBaseDirection;

    public float reloadTime = 2f;
    private float reloadTimer = 0f;

    public void Initialize(ShipCannonMultiSide manager)
    {
        this.manager = manager;
        // ѕолучаем базовое направление дл€ этой стороны от менеджера
        sideBaseDirection = manager.GetSideBaseDirection(side);
    }

    void Update()
    {
        if (reloadTimer > 0f)
        {
            reloadTimer -= Time.deltaTime;
        }
    }

    public void RotateToTarget(Vector3 worldTarget)
    {
        if (transform.parent == null) return;

        Vector3 dirToTargetWorld = worldTarget - transform.position;
        dirToTargetWorld.y = 0f;

        if (dirToTargetWorld.sqrMagnitude < 0.0001f) return;
        dirToTargetWorld.Normalize();

        // ѕреобразуем мировое направление в локальные координаты родител€
        Vector3 localDirToTarget = transform.parent.InverseTransformDirection(dirToTargetWorld);

        // –ассчитываем угол относительно базового направлени€, завис€щего от стороны
        float angle = Vector3.SignedAngle(sideBaseDirection, localDirToTarget, Vector3.up);
        float clampedAngle = Mathf.Clamp(angle, -maxLeftRotation, maxRightRotation);

        Quaternion targetLocalRotation = Quaternion.AngleAxis(clampedAngle, Vector3.up);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetLocalRotation, rotationSpeed * Time.deltaTime);
    }

    public bool IsWithinRotationLimits(Vector3 worldTargetPoint, out Vector3 shootDirection)
    {
        if (transform.parent == null)
        {
            shootDirection = transform.forward;
            return false;
        }

        Vector3 dirToTargetWorld = worldTargetPoint - transform.position;
        dirToTargetWorld.y = 0f;
        if (dirToTargetWorld.sqrMagnitude < 0.0001f)
        {
            shootDirection = transform.forward;
            return false;
        }
        dirToTargetWorld.Normalize();

        Vector3 localDirToTarget = transform.parent.InverseTransformDirection(dirToTargetWorld);

        float angle = Vector3.SignedAngle(sideBaseDirection, localDirToTarget, Vector3.up);

        bool inLimits = angle >= -maxLeftRotation && angle <= maxRightRotation;

        if (inLimits)
        {
            shootDirection = dirToTargetWorld;
        }
        else
        {
            float clampedAngle = Mathf.Clamp(angle, -maxLeftRotation, maxRightRotation);
            Quaternion rotationAtLimit = Quaternion.AngleAxis(clampedAngle, Vector3.up);
            shootDirection = transform.parent.TransformDirection(rotationAtLimit * sideBaseDirection);
        }

        return inLimits;
    }

    public bool TryShoot()
    {
        if (reloadTimer > 0f) return false;
        reloadTimer = reloadTime;
        return true;
    }
}