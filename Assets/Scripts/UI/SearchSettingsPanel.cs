using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI
{
    [RequireComponent(typeof(Button))]
    public class SearchSettingsPanel : MonoBehaviour
    {
        [SerializeField] private Button settingsButton;
        [SerializeField] private GameObject settingsPanel = null;

        private void Awake()
        {
            if(settingsButton == null)
            {
                settingsButton = GetComponent<Button>();
            }

            settingsPanel = GlobalAudioManager.Instance.settingsCanvas;
            
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
                settingsButton.onClick.AddListener(() => OpenSettings());
            }
        }

        private void OpenSettings()
        {
            settingsPanel.SetActive(true);
        }
    }
}