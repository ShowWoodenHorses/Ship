using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Assets.Scripts.Game
{
    public class StartGameManager : MonoBehaviour
    {

        [SerializeField] private GameObject ContinueButton;
        [SerializeField] private GameObject ConfirmationPanel;
        private void Awake()
        {
            ContinueButton.SetActive(false);
            ConfirmationPanel.SetActive(false);

            if (SaveSystem.IsExistsSave())
            {
                ContinueButton.SetActive(true);
            }
        }

        public void LoadGame()
        {
            SceneManager.LoadScene("SampleScene");

        }

        public void NewGame()
        {
            SaveSystem.New();
            SceneManager.LoadScene("SampleScene");
        }

        public void ShowConfirmationOrNewGame()
        {
            if (SaveSystem.IsExistsSave())
            {
                ConfirmationPanel.SetActive(true);
            }
            else
            {
                NewGame();
            }
        }
    }
}