using Assets.Scripts.Animation;
using Assets.Scripts.Scene;
using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Game
{
    public class StartGameManager : MonoBehaviour
    {
        [SerializeField] private GameObject ContinueButton;
        [SerializeField] private GameObject ConfirmationPanel;

        [SerializeField] private AudioSettingsManager audioSettingsManager;
        [SerializeField] private SoundPoolManager soundPoolManager;
        [SerializeField] private AudioMixer audioMixer;
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
            soundPoolManager.Initialize(audioMixer);
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