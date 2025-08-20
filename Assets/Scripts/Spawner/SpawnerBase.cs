using System.Collections;
using Assets.Scripts.Interface;
using UnityEngine;

namespace Assets.Scripts.Spawner
{
    public abstract class SpawnerBase : MonoBehaviour, ISpawner
    {
        public abstract bool HaveThisPrefab(GameObject prefab);

        public abstract GameObject Spawn(Transform playerTransform, GameObject prefab);
    }
}