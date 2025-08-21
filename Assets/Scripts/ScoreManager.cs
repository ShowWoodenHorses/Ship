using System;
using System.Collections;
using Assets.Scripts.Configs;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int currentMoney;
        [SerializeField] private int allTimeMoney;
        [SerializeField] private EnemyWaveMinValues enemyWaveMinValues;

        public Action<string> OnUpdateWave;

        public void AddMoney(int amount)
        {
            currentMoney += amount;
            allTimeMoney += amount;
            CheckAndSendForUpdate();
        }

        public void RemoveMoney(int amount)
        {
            currentMoney -= amount;
        }

        public int GetCurrentMoney()
        {
            return currentMoney;
        }

        public int GetAllTimeMoney()
        {
            return allTimeMoney;
        }

        private void CheckAndSendForUpdate()
        {
            if (enemyWaveMinValues.minValueEnemies.Length == 0) return;

            string currentWaveId;
            for (int i = enemyWaveMinValues.minValueEnemies.Length - 1; i >= 0; i--)
            {
                if (allTimeMoney >= enemyWaveMinValues.minValueEnemies[i].minScore)
                {
                    currentWaveId = enemyWaveMinValues.minValueEnemies[i].enemyWaveId;
                    OnUpdateWave?.Invoke(currentWaveId);
                    break;
                }
            }
        }
    }
}