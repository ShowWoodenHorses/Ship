using System.Text;
using Assets.Scripts;
using UnityEngine;

[ExecuteAlways]
public class EnemyCannon : MonoBehaviour
{
    public enum DebugLevel { None = 0, Minimal = 1, Full = 2 }

    [Header("Общее")]
    public EnemyWeaponSystem.CannonSide side = EnemyWeaponSystem.CannonSide.None;

    [Header("Трансформы")]
    [Tooltip("Трансформ, который должен вращаться (pivot). Если пусто - используется сам объект.")]
    public Transform pivot;
    [Tooltip("Точка/объект, откуда летит снаряд и чей forward используется для проверки попадания. Если пусто - попытаемся взять первый дочерний.")]
    public Transform barrel;

    [Header("Ось прицеливания (локально на pivot)")]
    [Tooltip("Локальная ось на pivot, которую считаем 'вперёд' (обычно (0,0,1)). Если AutoDetectAxis = true, будет подобрана автоматически.")]
    public Vector3 aimForwardLocal = Vector3.forward;
    public bool autoDetectAxis = true;

    [Header("Поворот и стрельба")]
    public float rotationSpeed = 60f;   // deg/sec
    public float maxLeftRotation = 45f;
    public float maxRightRotation = 45f;
    public float fireRadius = 50f;
    public float reloadTime = 3f;
    public float aimToleranceDeg = 6f;

    [Header("Debug")]
    public DebugLevel debugLevel = DebugLevel.Full;
    public float debugLogInterval = 0.25f;
    public bool drawRuntimeRays = true;
    public bool drawGizmos = true;

    // --- внутреннее состояние ---
    private Transform _target;
    private GameObject _projectilePrefab;
    private float _projectileSpeed;
    private float _reloadTimer;
    private float _debugTimer;

    // pivot-related
    private Transform _pivot;
    private Transform _barrel;
    private Quaternion _initialLocalRot;       // локальная ориентация pivot в Awake (базовая)
    private Vector3 _zeroForwardLocal;         // базовый forward (в системе координат родителя pivot)
    private readonly StringBuilder _sb = new StringBuilder(512);

    public void Initialize(Transform target, GameObject projectilePrefab, float projectileSpeed)
    {
        _target = target;
        _projectilePrefab = projectilePrefab;
        _projectileSpeed = projectileSpeed;
    }

    private void Awake()
    {
        _pivot = pivot != null ? pivot : transform;
        _barrel = barrel != null ? barrel : (_pivot.childCount > 0 ? _pivot.GetChild(0) : _pivot);
        _initialLocalRot = _pivot.localRotation;

        // Auto-detect aim axis if requested
        if (autoDetectAxis && _barrel != null)
            AutoDetectAimAxis();

        // compute zero forward local (в локальной системе родителя pivot)
        _zeroForwardLocal = _initialLocalRot * aimForwardLocal;
    }

    private void AutoDetectAimAxis()
    {
        // Попытаемся понять, какая локальная ось pivot соответствует forward барреля
        // Кандидаты: +Z, -Z, +X, -X
        Vector3[] candidates = new Vector3[] { Vector3.forward, -Vector3.forward, Vector3.right, -Vector3.right };
        string[] names = new string[] { "+Z", "-Z", "+X", "-X" };

        float bestDot = -Mathf.Infinity;
        Vector3 best = Vector3.forward;
        int bestIndex = 0;

        for (int i = 0; i < candidates.Length; i++)
        {
            // candidate в мировом направлении:
            Vector3 candWorld = _pivot.TransformDirection(candidates[i]); // локальная ось -> world
            // сравним с forward барабеля (мировой)
            float dot = Vector3.Dot(candWorld.normalized, _barrel.forward.normalized);
            if (dot > bestDot)
            {
                bestDot = dot;
                best = candidates[i];
                bestIndex = i;
            }
        }

        aimForwardLocal = best;
    }

    private void Update()
    {
        if (_reloadTimer > 0f) _reloadTimer -= Time.deltaTime;
        if (_target == null) return;

        RotateToTarget();
        TryFire();

        
    }

    private void RotateToTarget()
    {
        if (_pivot == null || _target == null) return;

        // Работаем в локальной системе родителя pivot (если родитель есть)
        Transform parent = _pivot.parent;
        Vector3 localPivot = _pivot.localPosition;
        Vector3 localTarget;
        if (parent != null)
            localTarget = parent.InverseTransformPoint(_target.position);
        else
            localTarget = _target.position; // fallback, world coords

        Vector3 localDelta = localTarget - localPivot;
        localDelta.y = 0f;

        if (localDelta.sqrMagnitude < 0.0001f) return;

        // Целевой угол: от базового forward (zeroForwardLocal) к направлению на цель
        Vector3 localDeltaDir = localDelta.normalized;
        float targetAngle = Vector3.SignedAngle(_zeroForwardLocal, localDeltaDir, Vector3.up);

        // Текущий локальный forward pivot
        Vector3 currentForwardLocal = _pivot.localRotation * aimForwardLocal;
        float currentAngle = Vector3.SignedAngle(_zeroForwardLocal, currentForwardLocal, Vector3.up);

        // Плавный поворот к целевому углу
        float newAngle = Mathf.MoveTowardsAngle(currentAngle, targetAngle, rotationSpeed * Time.deltaTime);

        // Клэмп в пределах сектора
        float clampedAngle = Mathf.Clamp(newAngle, -maxLeftRotation, maxRightRotation);

        // Применяем: поворачиваем pivot относительно его начальной локальной ориентации
        _pivot.localRotation = Quaternion.AngleAxis(clampedAngle, Vector3.up) * _initialLocalRot;
    }

    private void TryFire()
    {
        if (_reloadTimer > 0f) return;
        if (_barrel == null || _target == null) return;

        float dist = Vector3.Distance(_barrel.position, _target.position);
        if (dist > fireRadius) return;

        Vector3 dirToTarget = (_target.position - _barrel.position).normalized;
        float worldAngle = Vector3.Angle(_barrel.forward, dirToTarget);

        bool inAim = worldAngle <= aimToleranceDeg;

        if (inAim)
        {
            Fire();
        }
    }

    private void Fire()
    {
        if (_projectilePrefab == null) return;
        if (_barrel == null) _barrel = _pivot;

        var bullet = BulletObjectPool.Instance.GetObject(_projectilePrefab);
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(_barrel.position, Quaternion.LookRotation(_barrel.forward));
            BulletContoller bulletController = bullet.GetComponent<BulletContoller>();
            if (bulletController != null)
            {
                bulletController.Initialize(_barrel.forward);
            }
        }
        _reloadTimer = reloadTime;
    }

    private void DrawRuntimeDebug()
    {
        if (_barrel == null || _pivot == null || _target == null) return;

        Vector3 p = _barrel.position;
        Debug.DrawLine(p, p + _barrel.forward * 6f, Color.green); // barrel forward
        Debug.DrawLine(p, _target.position, Color.yellow); // to target

        // zero forward in world:
        Vector3 zeroWorld = _pivot.parent != null ? _pivot.parent.TransformDirection(_zeroForwardLocal) : _pivot.TransformDirection(_zeroForwardLocal);
        Debug.DrawLine(p, p + zeroWorld * 5f, new Color(0.2f, 0.6f, 1f));

        // sector bounds
        Vector3 left = _pivot.parent != null ? _pivot.parent.TransformDirection(Quaternion.AngleAxis(-maxLeftRotation, Vector3.up) * _zeroForwardLocal)
                                             : Quaternion.AngleAxis(-maxLeftRotation, Vector3.up) * zeroWorld;
        Vector3 right = _pivot.parent != null ? _pivot.parent.TransformDirection(Quaternion.AngleAxis(maxRightRotation, Vector3.up) * _zeroForwardLocal)
                                              : Quaternion.AngleAxis(maxRightRotation, Vector3.up) * zeroWorld;

        Debug.DrawLine(p, p + left * 5.5f, Color.cyan);
        Debug.DrawLine(p, p + right * 5.5f, Color.cyan);
    }

    private void LogState()
    {
        if (_target == null || _pivot == null) return;

        Transform parent = _pivot.parent;
        Vector3 localPivot = _pivot.localPosition;
        Vector3 localTarget = parent != null ? parent.InverseTransformPoint(_target.position) : _target.position;
        Vector3 localDelta = localTarget - localPivot;
        Vector3 localDeltaFlat = new Vector3(localDelta.x, 0f, localDelta.z);

        float targetAngle = localDeltaFlat.sqrMagnitude > 0.0001f
            ? Vector3.SignedAngle(_zeroForwardLocal, localDeltaFlat.normalized, Vector3.up)
            : 0f;

        Vector3 currentForwardLocal = _pivot.localRotation * aimForwardLocal;
        float currentAngle = Vector3.SignedAngle(_zeroForwardLocal, currentForwardLocal, Vector3.up);

        Vector3 dirToTargetWorld = (_target.position - (_barrel != null ? _barrel.position : _pivot.position));
        float dist = dirToTargetWorld.magnitude;
        dirToTargetWorld.Normalize();
        float worldAngle = Vector3.Angle((_barrel != null ? _barrel.forward : _pivot.forward), dirToTargetWorld);

        bool inRadius = dist <= fireRadius;
        bool inAim = worldAngle <= aimToleranceDeg;

        _sb.Clear();
        _sb.AppendLine($"[CANNON DEBUG] '{name}' (side={side})");
        _sb.AppendLine($"  Parent      : {(parent != null ? parent.name : "NULL")}, parentPos={(parent != null ? parent.position.ToString("F3") : "—")}, parentRot={(parent != null ? parent.eulerAngles.ToString("F1") : "—")}, parentScale={(parent != null ? parent.lossyScale.ToString("F3") : "—")}");
        _sb.AppendLine($"  Pivot (world): {_pivot.position.ToString("F3")}, localPivot={localPivot.ToString("F3")}");
        _sb.AppendLine($"  Barrel      : {(_barrel != null ? _barrel.name : "NULL")}, pos={(_barrel != null ? _barrel.position.ToString("F3") : "—")}");
        _sb.AppendLine($"  Target Pos  : {_target.position.ToString("F3")}");
        _sb.AppendLine($"  Local Target: {localTarget.ToString("F3")}");
        _sb.AppendLine($"  Local Delta : {localDelta.ToString("F3")} (flat={localDeltaFlat.ToString("F3")})");
        _sb.AppendLine($"  Aim axis loc: {aimForwardLocal.ToString("F3")}, zeroFwdLocal={_zeroForwardLocal.ToString("F3")}");
        _sb.AppendLine($"  Angles (deg): target={targetAngle:F2}, current={currentAngle:F2}, worldAim={worldAngle:F2}");
        _sb.AppendLine($"  Limits (deg): left={-maxLeftRotation:F1}, right={maxRightRotation:F1}, tol={aimToleranceDeg:F1}");
        _sb.AppendLine($"  Distance    : {dist:F2} / fireRadius={fireRadius:F2}");
        _sb.AppendLine($"  Can Fire?   : inRadius={inRadius}, inAim={inAim} => {(inRadius && inAim ? "YES" : "NO")}");
        Debug.Log(_sb.ToString(), this);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (!drawGizmos) return;
        Transform p = pivot != null ? pivot : transform;
        Transform b = barrel != null ? barrel : (p.childCount > 0 ? p.GetChild(0) : p);

        Vector3 pos = (b != null ? b.position : p.position);

        // zero forward world
        Quaternion initialLocal = Application.isPlaying ? _initialLocalRot : p.localRotation;
        Vector3 zeroFwdLocalNow = Application.isPlaying ? _zeroForwardLocal : (initialLocal * aimForwardLocal);
        Vector3 zeroFwdWorld = p.parent != null ? p.parent.TransformDirection(zeroFwdLocalNow) : p.TransformDirection(zeroFwdLocalNow);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(pos, pos + zeroFwdWorld * 4.5f);

        Vector3 leftWorld = p.parent != null
            ? p.parent.TransformDirection(Quaternion.AngleAxis(-maxLeftRotation, Vector3.up) * zeroFwdLocalNow)
            : Quaternion.AngleAxis(-maxLeftRotation, Vector3.up) * zeroFwdWorld;

        Vector3 rightWorld = p.parent != null
            ? p.parent.TransformDirection(Quaternion.AngleAxis(maxRightRotation, Vector3.up) * zeroFwdLocalNow)
            : Quaternion.AngleAxis(maxRightRotation, Vector3.up) * zeroFwdWorld;

        Gizmos.color = Color.blue;
        Gizmos.DrawLine(pos, pos + leftWorld * 5f);
        Gizmos.DrawLine(pos, pos + rightWorld * 5f);

        Gizmos.color = Color.green;
        if (b != null) Gizmos.DrawLine(pos, pos + b.forward * 4.5f);
    }
#endif
}
