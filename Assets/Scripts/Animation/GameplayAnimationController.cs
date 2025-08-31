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
            // Убиваем старое покачивание, если было
            swayTween?.Kill();

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
            sailUp.SetActive(true);
            sailUp.transform.localScale = Vector3.one;
            sailDown.transform.localScale = Vector3.zero;

            sailDown.SetActive(true);

            sailSequence = DOTween.Sequence()
                .Append(sailUp.transform.DOScale(Vector3.zero, transitionTime).SetEase(Ease.InOutSine))
                .Join(sailDown.transform.DOScale(Vector3.one, transitionTime).SetEase(Ease.InOutSine))
                .OnComplete(() => sailUp.SetActive(false))
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
