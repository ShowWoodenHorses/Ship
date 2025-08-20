using UnityEditor;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class EnemyType
    {
        public GameObject prefab;
        public int maxCount;

        public int currentCount;
    }
}