using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Animation
{
    public class GameplayAnimationController : MonoBehaviour
    {
        private Sequence sailSequence;
        private Tween swayTween;

        public void ShipSway(Transform childTransform, float wobbleAmount, float wobbleDuration)
        {
            // убиваем старое покачивание
            swayTween?.Kill();

            // ставим начальный угол влево
            childTransform.localRotation = Quaternion.Euler(0, 0, -wobbleAmount);

            // анимируем к +wobbleAmount и обратно
            swayTween = childTransform
                .DOLocalRotate(new Vector3(0, 0, wobbleAmount), wobbleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(childTransform.gameObject);
        }
        public void LowerSails(GameObject sailDown, GameObject sailUp, float transitionTime)
        {
            // Убиваем старую последовательность, если была
            sailSequence?.Kill();

            sailDown.SetActive(false);
            sailDown.transform.localScale = Vector3.zero;

            sailDown.SetActive(true);

            sailSequence = DOTween.Sequence()
                .Join(sailDown.transform.DOScale(Vector3.one, transitionTime).SetEase(Ease.InOutSine))
                .SetLink(sailDown); // Привязываем к объекту
        }

        public void PlayRecoil(Transform cannonVisual, float recoilDistance, float recoilDuration, float returnDuration, Vector3 direction)
        {
            Vector3 startLocalPos = cannonVisual.localPosition;

            cannonVisual.DOLocalMove(startLocalPos - direction * recoilDistance, recoilDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    cannonVisual.DOLocalMove(startLocalPos, returnDuration).SetEase(Ease.OutQuad);
                })
                .SetLink(cannonVisual.gameObject);
        }

        public Sequence DestroyShip(Transform transform)
        {
            sailSequence = DOTween.Sequence()
                .Join(transform.transform.DOMoveY(transform.position.y - 20f, 3f))
                .SetLink(transform.gameObject); // Привязываем к объекту

            return sailSequence;
        }

        /// <summary>
        /// Анимация покачивания для противников
        /// </summary>
        public Tween EnemySway(Transform childTransform, float wobbleAmount, float wobbleDuration)
        {
            // ставим начальный угол влево
            childTransform.localRotation = Quaternion.Euler(0, 0, -wobbleAmount);

            Tween swayTween = childTransform
                .DOLocalRotate(new Vector3(0, 0, wobbleAmount), wobbleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(childTransform.gameObject);

            return swayTween;
        }

        /// <summary>
        /// Полностью удалить все анимации у объекта
        /// </summary>
        public void DeleteAnimations(GameObject obj)
        {
            if (obj != null)
            {
                DOTween.Kill(obj); // убивает все твины, связанные с объектом через SetLink
            }
        }
    }
}
