using Assets.Scripts.Animation;
using Assets.Scripts.Scene;
using Assets.Scripts.Sound;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class StartGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject ContinueButton;
        [SerializeField] private GameObject ConfirmationPanel;

        [SerializeField] private AudioSettingsManager audioSettingsManager;
        private void Awake()
        {
            ContinueButton.SetActive(false);
            ConfirmationPanel.SetActive(false);

            if (SaveSystem.IsExistsSave())
            {
                ContinueButton.SetActive(true);
            }
        }

        private void Start()
        {
            audioSettingsManager.Initialize();
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