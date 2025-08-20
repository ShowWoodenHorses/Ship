using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "EnemyWaveConfig", menuName = "Ships/EnemyWaveConfig")]
    public class EnemyWaveConfig : ScriptableObject
    {
        public string id;

        public EnemyType[] enemyTypes;

        public int maxLimitEnemy;
        public int intervalSpawn;
    }
}