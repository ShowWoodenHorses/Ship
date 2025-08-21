using UnityEngine;

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
            systemPaused = !hasFocus;
            UpdatePause();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            systemPaused = pauseStatus;
            UpdatePause();
        }

        private void UpdatePause()
        {
            bool shouldPause = userPaused || systemPaused;

            Time.timeScale = shouldPause ? 0f : 1f;
            AudioListener.pause = shouldPause;

        }

        // Вызывается кнопкой
        public void Pause()
        {
            userPaused = true;
            UpdatePause();
        }

        // Вызывается кнопкой
        public void Resume()
        {
            userPaused = false;
            UpdatePause();
        }
    }
}
