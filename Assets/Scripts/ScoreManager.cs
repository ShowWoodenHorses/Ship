using System;
using System.Collections;
using Assets.Scripts.Configs;
using Assets.Scripts.Save;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int currentMoney;
        [SerializeField] private int allTimeMoney;
        [SerializeField] private EnemyWaveMinValues enemyWaveMinValues;

        public Action<string> OnUpdateWave;

        public void Initialize(int currentMoney, int allMoney)
        {
            this.currentMoney = currentMoney;
            this.allTimeMoney = allMoney;
        }

        public void AddMoney(int amount)
        {
            currentMoney += amount;
            allTimeMoney += amount;
            SaveLifecycle.instance.AddMoney(currentMoney, allTimeMoney);
            CheckAndSendForUpdate();
        }

        public void RemoveMoney(int amount)
        {
            currentMoney -= amount;
            SaveLifecycle.instance.RemoveMoney(currentMoney);
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
                    SaveLifecycle.instance.ChangeWave(currentWaveId);
                    OnUpdateWave?.Invoke(currentWaveId);
                    break;
                }
            }
        }
    }
}