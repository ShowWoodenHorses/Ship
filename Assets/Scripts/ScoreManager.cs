using System.Collections;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField] private int currentScore;
        [SerializeField] private int allTimeScore;

        public void AddScore(int smount)
        {
            currentScore += smount;
            allTimeScore += smount;
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
    }
}