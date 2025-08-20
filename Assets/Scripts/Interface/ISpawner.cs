using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Interface
{
    public interface ISpawner
    {
        bool HaveThisPrefab(GameObject prefab);
        GameObject Spawn(Transform playerTransform, GameObject prefab);
    }
}