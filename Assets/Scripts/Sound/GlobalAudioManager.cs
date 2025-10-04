using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Sound
{
    public class GlobalAudioManager : MonoBehaviour
    {
        public static GlobalAudioManager Instance;

        private bool userMuted = false;   // mute по кнопке
        private bool systemMuted = false; // mute при потере фокуса

        [SerializeField] private GameObject activeSoundImage;
        [SerializeField] private GameObject disableSoundImage;

        private const string MuteKey = "muteSound";

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            // загружаем сохранённое значение mute игрока
            userMuted = PlayerPrefs.GetInt(MuteKey, 0) == 1;
            ApplyState();
        }

        public void ToggleUserMute()
        {
            userMuted = !userMuted;
            PlayerPrefs.SetInt(MuteKey, userMuted ? 1 : 0);
            PlayerPrefs.Save();
            ApplyState();
        }

        public void SetSystemMute(bool value)
        {
            systemMuted = value;
            ApplyState();
        }

        private void ApplyState()
        {
            bool finalMute = userMuted || systemMuted;

            AudioListener.pause = finalMute;
            if (activeSoundImage != null) activeSoundImage.SetActive(!userMuted);
            if (disableSoundImage != null) disableSoundImage.SetActive(userMuted);
        }
    }
}