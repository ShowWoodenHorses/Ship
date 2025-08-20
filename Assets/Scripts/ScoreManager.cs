using System;
using System.Collections;
using Assets.Scripts.Configs;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int currentScore;
        [SerializeField] private int allTimeScore;
        [SerializeField] private EnemyWaveMinValues enemyWaveMinValues;

        public Action<string> OnUpdateWave;

        public void AddScore(int smount)
        {
            currentScore += smount;
            allTimeScore += smount;
            CheckAndSendForUpdate();
        }

        public void RemoveScore(int smount)
        {
            currentScore -= smount;
        }

        public int GetCurrentScore()
        {
            return currentScore;
        }

        public int GetAllTimeScore()
        {
            return allTimeScore;
        }

        private void CheckAndSendForUpdate()
        {
            if (enemyWaveMinValues.minValueEnemies.Length == 0) return;

            string currentWaveId;
            for (int i = enemyWaveMinValues.minValueEnemies.Length - 1; i >= 0; i--)
            {
                if (allTimeScore >= enemyWaveMinValues.minValueEnemies[i].minScore)
                {
                    currentWaveId = enemyWaveMinValues.minValueEnemies[i].enemyWaveId;
                    OnUpdateWave?.Invoke(currentWaveId);
                    break;
                }
            }
        }
    }
}