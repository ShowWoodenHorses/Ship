using System;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "EnemyWaveMinValues", menuName = "Ships/EnemyWaveMinValues")]
    public class EnemyWaveMinValues : ScriptableObject
    {
        [Serializable]
        public class MinValueEnemy
        {
            public int minScore;
            public string enemyWaveId;
        }

        public MinValueEnemy[] minValueEnemies;

    }
}