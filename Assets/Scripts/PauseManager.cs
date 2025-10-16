using Assets.Scripts.Sound;
using UnityEngine;
using YG;

namespace Assets.Scripts
{
    public class PauseManager : MonoBehaviour
    {
        private bool userPaused = false;    // пауза через кнопку
        private bool systemPaused = false;  // пауза из-за потери фокуса

        private void Start()
        {
            Resume();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            GlobalAudioManager.Instance.SetSystemMute(!hasFocus);
            systemPaused = !hasFocus;
            UpdatePause();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            GlobalAudioManager.Instance.SetSystemMute(pauseStatus);
            systemPaused = pauseStatus;
            UpdatePause();
        }

        private void UpdatePause()
        {
            bool shouldPause = userPaused || systemPaused;

            Time.timeScale = shouldPause ? 0f : 1f;

        }

        public void Pause()
        {
            userPaused = true;
            YG2.GameplayStop();
            UpdatePause();
        }

        public void Resume()
        {
            userPaused = false;
            YG2.GameplayStart();
            UpdatePause();
        }
    }
}
