using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace Assets.Scripts.Scene
{
    public class LoadingScreen : MonoBehaviour
    {
        [Header("UI элементы")]
        public Image progressBar;          // спрайт полосы (Image, type = Filled)
        public Image progressItem;          // спрайт полосы (Image, type = Filled)
        public TextMeshProUGUI progressText; // проценты

        private static string sceneToLoad; // сюда передадим имя сцены

        public static void LoadScene(string sceneName)
        {
            sceneToLoad = sceneName;
            SceneManager.LoadScene("LoadingScene"); // грузим сцену загрузки
        }

        private void Start()
        {
            StartCoroutine(LoadAsync());
        }

        private IEnumerator LoadAsync()
        {
            yield return null;

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
            op.allowSceneActivation = false; // ждём полной загрузки

            while (!op.isDone)
            {
                // Прогресс идёт до 0.9, потом ждёт активации
                float progress = Mathf.Clamp01(op.progress / 0.9f);

                if (progressBar != null)
                    progressBar.fillAmount = progress;

                if (progressText != null)
                    progressText.text = Mathf.RoundToInt(progress * 100f) + "%";

                if (progressItem != null)
                {
                    RectTransform barRect = progressBar.GetComponent<RectTransform>();
                    RectTransform itemRect = progressItem.GetComponent<RectTransform>();

                    // ширина заполненной части
                    float barWidth = barRect.rect.width;

                    // смещение от левого края
                    float newX = -barWidth / 2f + barWidth * progress;

                    // обновляем локальную позицию иконки
                    itemRect.anchoredPosition = new Vector3(newX, itemRect.localPosition.y, itemRect.localPosition.z);
                }

                // Если сцена загрузилась на 90% → активируем
                if (op.progress >= 0.9f)
                {
                    // можно сделать задержку/анимацию
                    op.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}