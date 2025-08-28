using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Animation
{    public class ShipSway : MonoBehaviour
    {
        public Transform shipVisual;   // дочерний объект (модель корабля)
        public float wobbleAmount = 5f; // амплитуда качки
        public float wobbleDuration = 2f; // длительность качки в одну сторону

        private Tween anim;

        public void Initialize(Transform childTransform)
        {
            shipVisual = childTransform;

            anim = shipVisual
                .DOLocalRotate(new Vector3(0, 0, wobbleAmount), wobbleDuration)
                .SetEase(Ease.InOutSine)
                .SetLoops(-1, LoopType.Yoyo);
        }

        void OnDestroy()
        {
            if (anim != null)
                anim.Kill();
        }
    }
}