using Assets.Scripts.Animation;
using Assets.Scripts.Scene;
using UnityEngine;

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
            LoadingScreen.LoadScene("SampleScene");

        }

        public void NewGame()
        {
            SaveSystem.New();
            LoadingScreen.LoadScene("SampleScene");
        }

        public void ShowConfirmationOrNewGame()
        {
            if (SaveSystem.IsExistsSave())
            {
                ConfirmationPanel.GetComponent<HopupAnimUI>().Hopup();
            }
            else
            {
                NewGame();
            }
        }
    }
}