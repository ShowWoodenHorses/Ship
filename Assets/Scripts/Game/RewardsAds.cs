using System.Collections;
using UnityEngine;
using YG;

namespace Assets.Scripts.Game
{
    public class RewardsAds : MonoBehaviour
    {
        [SerializeField] private string coinsId = "coins";

        [SerializeField] private int countMoneyReward = 100;

        private ScoreManager scoreManager;

        public void Initialize(ScoreManager scoreManager)
        {
            this.scoreManager = scoreManager;
            YG2.onRewardAdv += Reward;
        }

        private void RewardCoin()
        {
            YG2.MetricaSend("showRewardAds");
            scoreManager.AddMoney(countMoneyReward);
        }

        private void Reward(string id)
        {
            if (id == coinsId)
            {
                RewardCoin();
            }
        }

        private void OnDisable()
        {
            YG2.onRewardAdv -= Reward;
        }
    }
}