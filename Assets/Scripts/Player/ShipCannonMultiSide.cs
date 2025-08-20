using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Player;

public class ShipCannonMultiSide : MonoBehaviour
{
    public enum CannonSide { Left, Right, Front, Rear }

    [Header("Основные настройки")]
    public float arcAngle = 60f;

    [Header("Префабы и ссылки")]
    public GameObject cannonballPrefab;
    public SectorHighlightMesh sectorHighlightEffect;

    [Header("Line renderers")]
    public LineRenderer trajectoryLine;
    public LineRenderer activeCannonLaser;
    public float laserWidth = 0.06f;

    [Header("Параметры стрельбы")]
    public float projectileSpeed = 30f;
    public int trajectoryResolution = 30;
    public float timeStep = 0.06f;

    private Dictionary<CannonSide, List<ShipCannon>> cannons = new();
    private Dictionary<CannonSide, int> nextCannonIndex = new();

    private CannonSide currentActiveSide;
    private ShipCannon currentSelectedCannon;

    void Start()
    {
        InitializeCannons();
        SetupVisualEffects();
    }

    void InitializeCannons()
    {
        ShipCannon[] allCannons = GetComponentsInChildren<ShipCannon>(true);

        foreach (CannonSide s in System.Enum.GetValues(typeof(CannonSide)))
        {
            cannons[s] = new List<ShipCannon>();
            nextCannonIndex[s] = 0;
        }

        foreach (var c in allCannons)
        {
            c.Initialize(this);
            cannons[c.side].Add(c);
        }
    }

    public void InitializeBullet(GameObject bulletPrefab)
    {
        cannonballPrefab = bulletPrefab;
    }

    void SetupVisualEffects()
    {
        if (activeCannonLaser != null)
        {
            activeCannonLaser.startWidth = laserWidth;
            activeCannonLaser.endWidth = laserWidth;
            if (activeCannonLaser.material == null)
                activeCannonLaser.material = new Material(Shader.Find("Sprites/Default"));
            activeCannonLaser.enabled = false;
            activeCannonLaser.positionCount = 2;
        }

        if (trajectoryLine != null)
        {
            trajectoryLine.positionCount = 0;
        }
    }

    void Update()
    {
        Vector3 mouseWorld = GetMouseWorldPoint();
        currentActiveSide = DetermineActiveSide(mouseWorld);
        UpdateSectorVisualization(currentActiveSide);

        if (HasCannonsOnSide(currentActiveSide))
        {
            currentSelectedCannon = GetNextCannonForSide(currentActiveSide);
            RotateCannonsToTarget(currentActiveSide, mouseWorld);
            UpdateLaserAndTrajectoryForSelected(mouseWorld);

            if (Input.GetMouseButtonDown(0))
            {
                TryShootOnce(currentActiveSide, mouseWorld);
            }
        }
        else
        {
            ClearVisualEffects();
            currentSelectedCannon = null;
        }
    }

    CannonSide DetermineActiveSide(Vector3 mouseWorld)
    {
        Vector3 dirToMouse = (mouseWorld - transform.position).normalized;
        float frontDot = Vector3.Dot(transform.forward, dirToMouse);
        float rearDot = Vector3.Dot(-transform.forward, dirToMouse);
        float rightDot = Vector3.Dot(transform.right, dirToMouse);
        float leftDot = Vector3.Dot(-transform.right, dirToMouse);

        float max = frontDot;
        CannonSide side = CannonSide.Front;

        if (rearDot > max) { max = rearDot; side = CannonSide.Rear; }
        if (rightDot > max) { max = rightDot; side = CannonSide.Right; }
        if (leftDot > max) { max = leftDot; side = CannonSide.Left; }

        return side;
    }

    void UpdateSectorVisualization(CannonSide side)
    {
        if (sectorHighlightEffect == null) return;
        sectorHighlightEffect.transform.position = transform.position;
        sectorHighlightEffect.transform.rotation = transform.rotation * GetSideRotation(side);
        sectorHighlightEffect.SetActive(transform, side, this);
    }

    void RotateCannonsToTarget(CannonSide side, Vector3 mouseWorld)
    {
        foreach (var c in cannons[side])
        {
            c.RotateToTarget(mouseWorld);
        }
    }

    ShipCannon GetNextCannonForSide(CannonSide side)
    {
        if (!cannons.ContainsKey(side) || cannons[side].Count == 0) return null;
        int idx = nextCannonIndex[side] % cannons[side].Count;
        return cannons[side][idx];
    }

    void UpdateLaserAndTrajectoryForSelected(Vector3 mouseWorld)
    {
        if (currentSelectedCannon == null)
        {
            ClearVisualEffects();
            return;
        }

        Vector3 shootDir;
        bool inLimits = currentSelectedCannon.IsWithinRotationLimits(mouseWorld, out shootDir);

        Vector3 startPos = currentSelectedCannon.transform.position;

        if (activeCannonLaser != null)
        {
            activeCannonLaser.enabled = true;
            activeCannonLaser.SetPosition(0, startPos);
            if (inLimits)
            {
                activeCannonLaser.SetPosition(1, mouseWorld);
            }
            else
            {
                activeCannonLaser.SetPosition(1, startPos + shootDir * 50f);
            }
        }

        if (trajectoryLine != null)
        {
            ShowTrajectory(startPos, shootDir.normalized * projectileSpeed);
        }

        Debug.DrawRay(currentSelectedCannon.transform.position, currentSelectedCannon.transform.forward * 5f, Color.red);
    }

    void TryShootOnce(CannonSide side, Vector3 mouseWorld)
    {
        var sideList = cannons[side];
        if (sideList.Count == 0) return;

        int tries = sideList.Count;
        int idx = nextCannonIndex[side] % sideList.Count;

        while (tries > 0)
        {
            var cannon = sideList[idx];
            if (cannon.TryShoot())
            {
                cannon.IsWithinRotationLimits(mouseWorld, out Vector3 shootDir);
                FireCannon(cannon.ShotPos.position, shootDir);
                nextCannonIndex[side] = (idx + 1) % sideList.Count;
                return;
            }
            idx = (idx + 1) % sideList.Count;
            tries--;
        }
    }

    void FireCannon(Vector3 position, Vector3 direction)
    {
        if (cannonballPrefab == null) return;

        GameObject bullet = BulletObjectPool.Instance.GetObject(cannonballPrefab);
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(position, Quaternion.LookRotation(direction));
            BulletContoller bulletController = bullet.GetComponent<BulletContoller>();
            if (bulletController != null)
            {
                bulletController.Initialize(direction.normalized);
            }
        }
    }

    void ShowTrajectory(Vector3 origin, Vector3 velocity)
    {
        if (trajectoryLine == null) return;

        Vector3 pos = origin;
        Vector3 vel = velocity;

        Vector3[] points = new Vector3[trajectoryResolution];
        for (int i = 0; i < trajectoryResolution; i++)
        {
            points[i] = pos;
            vel += Physics.gravity * timeStep;
            pos += vel * timeStep;
        }

        trajectoryLine.positionCount = trajectoryResolution;
        trajectoryLine.SetPositions(points);
    }

    void ClearVisualEffects()
    {
        if (activeCannonLaser != null) activeCannonLaser.enabled = false;
        if (trajectoryLine != null) trajectoryLine.positionCount = 0;
    }

    Quaternion GetSideRotation(CannonSide side)
    {
        return side switch
        {
            CannonSide.Front => Quaternion.identity,
            CannonSide.Rear => Quaternion.Euler(0f, 180f, 0f),
            CannonSide.Right => Quaternion.Euler(0f, 90f, 0f),
            CannonSide.Left => Quaternion.Euler(0f, -90f, 0f),
            _ => Quaternion.identity,
        };
    }

    // Новая функция, которая предоставляет базовое направление для каждой стороны
    public Vector3 GetSideBaseDirection(CannonSide side)
    {
        return side switch
        {
            CannonSide.Front => Vector3.forward,
            CannonSide.Rear => Vector3.back,
            CannonSide.Right => Vector3.right,
            CannonSide.Left => Vector3.left,
            _ => Vector3.forward,
        };
    }

    Vector3 GetMouseWorldPoint()
    {
        if (Camera.main == null) return transform.position + transform.forward * 10f;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, LayerMask.GetMask("Water", "Ground")))
        {
            return hit.point;
        }

        // Резервный вариант, если луч не попал ни во что
        Plane groundPlane = new Plane(Vector3.up, transform.position.y);
        if (groundPlane.Raycast(ray, out float distance))
        {
            return ray.GetPoint(distance);
        }

        return ray.origin + ray.direction * 100f;
    }

    public bool HasCannonsOnSide(CannonSide side)
    {
        return cannons.ContainsKey(side) && cannons[side].Count > 0;
    }

    public float GetMaxAttackAngle(CannonSide side)
    {
        if (!HasCannonsOnSide(side)) return 0f;
        if (side == CannonSide.Front || side == CannonSide.Rear) return arcAngle;

        float maxAngle = 0f;
        foreach (var c in cannons[side])
        {
            maxAngle = Mathf.Max(maxAngle, Mathf.Max(c.maxLeftRotation, c.maxRightRotation));
        }
        return maxAngle;
    }
}