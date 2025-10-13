using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Assets.Scripts.Animation;
using YG;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private ScoreManager scoreManager;

        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        [SerializeField] private TextMeshProUGUI textMoney;
        [SerializeField] private TextMeshProUGUI textMoneyinStore;

        private void Update()
        {
            textMoney.text = scoreManager.GetCurrentMoney().ToString();
            textMoneyinStore.text = scoreManager.GetCurrentMoney().ToString();
        }

        public void PauseButton()
        {
            pauseManager.Pause();
        }

        public void ResumeButton()
        {
            pauseManager.Resume();
        }

        public void ShowWinPanel()
        {
            YG2.MetricaSend("win");
            winPanel.GetComponent<HopupAnimUI>().Hopup();
            pauseManager.Pause();
        }
        public void ShowLosePanel()
        {
            YG2.MetricaSend("gameOver");
            losePanel.GetComponent<HopupAnimUI>().Hopup();
            pauseManager.Pause();
        }

        public void HideLosePanel()
        {
            losePanel.SetActive(false);
            pauseManager.Resume();
        }
    }
}