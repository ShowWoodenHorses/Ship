using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private ScoreManager scoreManager;

        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

        [SerializeField] private TextMeshProUGUI textMoney;

        private void Update()
        {
            textMoney.text = scoreManager.GetCurrentMoney().ToString();
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
            winPanel.SetActive(true);
            pauseManager.Pause();
        }
        public void ShowLosePanel()
        {
            losePanel.SetActive(true);
            pauseManager.Pause();
        }

        public void ExitMenu()
        {
            SceneManager.LoadScene("StartScene");
        }
    }
}