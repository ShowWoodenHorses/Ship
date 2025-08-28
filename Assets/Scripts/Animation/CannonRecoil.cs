using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Animation
{    public class CannonRecoil : MonoBehaviour
    {
        public Transform cannonVisual;  // объект визуала ствола
        public float recoilDistance = 0.5f;
        public float recoilDuration = 0.1f;
        public float returnDuration = 0.2f;

        private Tween anim;

        public void PlayRecoil()
        {
            if (cannonVisual == null) cannonVisual = transform.GetChild(0);

            // Сбрасываем любые старые анимации
            cannonVisual.DOKill();

            Vector3 startLocalPos = cannonVisual.localPosition;

            // Откат назад вдоль -Z
            anim = cannonVisual.DOLocalMove(startLocalPos - Vector3.right * recoilDistance, recoilDuration)
                .SetEase(Ease.OutQuad)
                .OnComplete(() =>
                {
                    // Возврат
                    cannonVisual.DOLocalMove(startLocalPos, returnDuration).SetEase(Ease.OutQuad);
                });
        }

        private void OnDestroy()
        {
            if (anim != null)
            {
                anim.Kill();
            }
        }
    }
}