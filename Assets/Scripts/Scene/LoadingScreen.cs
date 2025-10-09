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
        public Image progressBar;           // Полоса прогресса (Image, type = Filled)
        public Image progressItem;          // Иконка, движущаяся по полосе
        public TextMeshProUGUI progressText; // Текст с процентами

        private static string sceneToLoad;
        private float fakeProgress = 0f;    // Псевдопрогресс (0–100)

        public static void LoadScene(string sceneName)
        {
            sceneToLoad = sceneName;
            SceneManager.LoadScene("LoadingScene"); // Сначала грузим сцену загрузки
        }

        private void Start()
        {
            StartCoroutine(LoadAsync());
        }

        private IEnumerator LoadAsync()
        {
            yield return null;

            AsyncOperation op = SceneManager.LoadSceneAsync(sceneToLoad);
            op.allowSceneActivation = false;

            float realProgress = 0f;

            while (!op.isDone)
            {
                // Прогресс Unity идёт до 0.9
                realProgress = Mathf.Clamp01(op.progress / 0.9f);

                // --- Плавный визуальный прогресс ---
                if (fakeProgress < realProgress * 100f)
                {
                    fakeProgress += Time.deltaTime * 30f; // скорость подъёма
                }
                else if (realProgress >= 0.9f)
                {
                    // Когда сцена реально почти загружена — дотягиваем до 100%
                    fakeProgress = Mathf.MoveTowards(fakeProgress, 100f, Time.deltaTime * 40f);
                }

                // Обновляем UI
                UpdateUI(fakeProgress / 100f);

                // Когда псевдопрогресс достиг 100 — активируем сцену
                if (fakeProgress >= 100f)
                {
                    op.allowSceneActivation = true;
                }

                yield return null;
            }
        }

        private void UpdateUI(float normalizedProgress)
        {
            if (progressBar != null)
                progressBar.fillAmount = normalizedProgress;

            if (progressText != null)
                progressText.text = Mathf.RoundToInt(normalizedProgress * 99f) + "%";

            if (progressItem != null && progressBar != null)
            {
                RectTransform barRect = progressBar.GetComponent<RectTransform>();
                RectTransform itemRect = progressItem.GetComponent<RectTransform>();

                float barWidth = barRect.rect.width;
                float newX = -barWidth / 2f + barWidth * normalizedProgress;

                itemRect.anchoredPosition = new Vector3(newX, itemRect.localPosition.y, itemRect.localPosition.z);
            }
        }
    }
}
