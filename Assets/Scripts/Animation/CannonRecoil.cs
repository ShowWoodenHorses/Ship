using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Animation
{    public class CannonRecoil : MonoBehaviour
    {
        public Transform cannonVisual;  // объект визуала ствола
        public float recoilDistance = 0.5f;
        public float recoilDuration = 0.1f;
        public float returnDuration = 0.2f;

        public Direction direction;

        private Tween anim;

        //public void PlayRecoil()
        //{
        //    if (cannonVisual == null) cannonVisual = transform.GetChild(0);

        //    // Сбрасываем любые старые анимации
        //    cannonVisual.DOKill();

        //    Vector3 startLocalPos = cannonVisual.localPosition;

        //    // Откат назад вдоль -Z
        //    anim = cannonVisual.DOLocalMove(startLocalPos - Vector3.right * recoilDistance, recoilDuration)
        //        .SetEase(Ease.OutQuad)
        //        .OnComplete(() =>
        //        {
        //            // Возврат
        //            cannonVisual.DOLocalMove(startLocalPos, returnDuration).SetEase(Ease.OutQuad);
        //        });
        //}

        //private void OnDestroy()
        //{
        //    if (anim != null)
        //    {
        //        anim.Kill();
        //    }
        //}

        public Vector3 GetDirection(Direction dir)
        {
            return dir switch
            {
                Direction.Front => Vector3.forward,
                Direction.Right => Vector3.right,
                Direction.Left => -Vector3.right,
                Direction.Forward => -Vector3.forward,
                _ => Vector3.right

            };
        }
    }

    public enum Direction
    {
        Front,
        Right,
        Forward,
        Left
    }


}