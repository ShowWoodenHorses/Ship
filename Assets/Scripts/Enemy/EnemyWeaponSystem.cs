using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class EnemyWeaponSystem : MonoBehaviour
{
    public enum CannonSide { None, Left, Right, Front, Rear }

    [Header("Боеприпасы")]
    public GameObject cannonballPrefab;
    public float projectileSpeed = 30f;

    [Header("Debug")]
    public bool debugEnabled = true;

    private Transform _currentTarget;
    private readonly List<EnemyCannon> _allCannons = new List<EnemyCannon>();

    private void Awake()
    {
        _allCannons.Clear();
        _allCannons.AddRange(GetComponentsInChildren<EnemyCannon>(true));

        if (debugEnabled)
        {
            var sb = new StringBuilder(256);
            foreach (var c in _allCannons)
            {
                if (c == null) continue;
            }
        }
    }

    public void SetTarget(Transform target)
    {
        _currentTarget = target;

        foreach (var cannon in _allCannons)
        {
            if (cannon == null) continue;
            cannon.Initialize(_currentTarget, cannonballPrefab, projectileSpeed);
        }

    }
}
