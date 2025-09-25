using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;

        [SerializeField] private GameObject winPanel;
        [SerializeField] private GameObject losePanel;

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