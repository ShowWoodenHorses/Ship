using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "EnemyWaveDatabase", menuName = "Ships/EnemyWaveDatabase")]
    public class EnemyWaveDatabase : ScriptableObject
    {
        public EnemyWaveConfig[] configs;

        public EnemyWaveConfig GetEnemyConfigById(string id)
        {
            return configs.FirstOrDefault(c => c.id == id);
        }
    }
}