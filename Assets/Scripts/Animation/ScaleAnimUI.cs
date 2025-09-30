using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Animation
{
    public class ScaleAnimUI : MonoBehaviour
    {
        public float delay = 0.1f;   // задержка перед пульсацией
        public float scaleFactor = 1.1f;
        public float duration = 0.5f;

        [SerializeField] private bool autoPlay = true;
        [SerializeField] private bool useUnscaledTime = true;

        private RectTransform rectTransform;
        private Tween tween;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void Start()
        {
            if (autoPlay) StartAnim();
        }

        private void OnDisable()
        {
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }
        }

        public void StartAnim()
        {
            if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
            if (rectTransform == null) return;

            // Если уже есть активный tween — ничего не делаем
            if (tween != null && tween.IsActive()) return;

            tween = rectTransform
                .DOScale(scaleFactor, duration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine)
                .SetDelay(delay);

            if (useUnscaledTime) tween.SetUpdate(true);
        }

        // (опционально) публичный Stop
        public void StopAnim()
        {
            if (tween != null)
            {
                tween.Kill();
                tween = null;
            }
            // вернуть scale в единицу, если нужно
            if (rectTransform != null) rectTransform.localScale = Vector3.one;
        }
    }
}
