using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Assets.Scripts.Sound;

namespace Assets.Scripts.Animation
{
    public class HopupAnimUI : MonoBehaviour
    {
        [SerializeField] private List<GameObject> children = new List<GameObject>();
        [SerializeField] private float duration = 0.5f;        // длительность анимации окна
        [SerializeField] private float childDuration = 0.3f;   // длительность анимации каждой кнопки
        [SerializeField] private float stagger = 0.1f;        // задержка (между кнопками или задержка перед массовым появлением)
        [SerializeField] private bool animateSequentially = true;
        [SerializeField] private float offsetY = -200f;
        [SerializeField] private bool useUnscaledTime = true;  // если true — анимации идут при Time.timeScale = 0

        [Header("Sound")]
        [SerializeField] private AudioSource soundHopupPrefab;

        private Sequence seq;

        public void Hopup()
        {
            // Остановка предыдущей последовательности, если есть
            if (seq != null && seq.IsActive())
            {
                seq.Kill();
                seq = null;
            }

            RectTransform parentRect = GetComponent<RectTransform>();
            if (parentRect == null)
            {
                Debug.LogWarning("HopupAnimUI: у родителя нет RectTransform.");
                return;
            }

            gameObject.SetActive(true);

            CanvasGroup parentCanvas = GetComponent<CanvasGroup>();
            if (parentCanvas == null)
                parentCanvas = gameObject.AddComponent<CanvasGroup>();

            // Подготовка окна (внизу и прозрачное)
            Vector2 targetAnchoredPos = parentRect.anchoredPosition;
            parentRect.anchoredPosition = targetAnchoredPos + new Vector2(0f, offsetY);
            parentCanvas.alpha = 0f;

            // Подготовка детей
            if (children != null)
            {
                foreach (var child in children)
                {
                    if (child == null) continue;
                    child.SetActive(true);
                    child.transform.localScale = Vector3.zero;
                }
            }

            // Создаём последовательность
            seq = DOTween.Sequence();
            if (useUnscaledTime)
            {
                seq.SetUpdate(true); // делает всю последовательность unscaled
            }

            SoundPoolManager.Instance.PlaySound(soundHopupPrefab);

            seq.Append(parentRect.DOAnchorPos(targetAnchoredPos, duration).SetEase(Ease.OutCubic));
            seq.Join(parentCanvas.DOFade(1f, duration));

            // Небольшая общая задержка перед появлением кнопок
            seq.AppendInterval(stagger);

            // Анимации кнопок
            if (children != null && children.Count > 0)
            {
                if (animateSequentially)
                {
                    // по очереди
                    for (int i = 0; i < children.Count; i++)
                    {
                        var child = children[i];
                        if (child == null) continue;

                        // захватываем локальную копию для лямбды
                        var capturedChild = child;
                        Transform tr = capturedChild.transform;

                        // создаём tween отдельный для кнопки
                        Tween childTween = tr.DOScale(Vector3.one, childDuration).SetEase(Ease.OutBack);
                        if (useUnscaledTime) childTween.SetUpdate(true);

                        // добавляем tween в последовательность (Append — последовательно)
                        seq.Append(childTween);

                        // вызывать StartAnim конкретно для этой кнопки, когда её tween завершится
                        childTween.OnComplete(() =>
                        {
                            var scaleAnim = capturedChild.GetComponent<ScaleAnimUI>();
                            if (scaleAnim != null)
                                scaleAnim.StartAnim();
                        });

                        // интервал между кнопками (если не последняя)
                        if (i < children.Count - 1)
                            seq.AppendInterval(stagger);
                    }
                }
                else
                {
                    // одновременно — все tween'ы джойним к sequence в текущий момент
                    foreach (var child in children)
                    {
                        if (child == null) continue;
                        var capturedChild = child;
                        Transform tr = capturedChild.transform;

                        Tween childTween = tr.DOScale(Vector3.one, childDuration).SetEase(Ease.OutBack);
                        if (useUnscaledTime) childTween.SetUpdate(true);

                        // Присоединяем tween к текущему положению sequence (join — параллельно)
                        seq.Join(childTween);

                        // Запустить StartAnim для этой конкретной кнопки при окончании её tween'а
                        childTween.OnComplete(() =>
                        {
                            var scaleAnim = capturedChild.GetComponent<ScaleAnimUI>();
                            if (scaleAnim != null)
                                scaleAnim.StartAnim();
                        });
                    }
                }
            }

            // (по желанию) можно подписаться на завершение всей последовательности:
            // seq.OnComplete(() => Debug.Log("Hopup полностью завершён"));
        }
    }
}
