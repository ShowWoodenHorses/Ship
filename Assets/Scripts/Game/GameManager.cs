using System.Collections.Generic;
using Assets.Scripts.Enemy;
using Assets.Scripts.Player;
using Assets.Scripts.Scene;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class GameManager : MonoBehaviour
    {
        public List<BuildController> buildings;

        [SerializeField] private int countDestroyedBuildingForWin;

        [SerializeField] private UIController uiController;
        [SerializeField] private ScoreManager scoreManager;

        public void Initialize(UIController uiController, ScoreManager scoreManager)
        {
            this.uiController = uiController;
            this.scoreManager = scoreManager;

            ShipHealth.OnPlayerDie += CheckPlayerHealth;

            foreach (var build in buildings)
            {
                build.Initialize();
                build.OnBuildingDestroyed += CheckCurrentCountBuildings;
            }

            countDestroyedBuildingForWin = buildings.Count;
        }

        private void CheckCurrentCountBuildings(BuildController buildController)
        {
            int reward = buildController.GetReward();
            scoreManager.AddMoney(reward);

            countDestroyedBuildingForWin--;
            if (countDestroyedBuildingForWin <= 0)
            {
                uiController.ShowWinPanel();
            }
        }

        private void CheckPlayerHealth(GameObject obj)
        {
            uiController.ShowLosePanel();
        }

        private void OnDisable()
        {
            foreach (var build in buildings)
            {
                build.OnBuildingDestroyed -= CheckCurrentCountBuildings;
            }

            ShipHealth.OnPlayerDie -= CheckPlayerHealth;
        }
        public void NewGame()
        {
            SaveSystem.New();
            LoadingScreen.LoadScene("SampleScene");
        }
    }
}