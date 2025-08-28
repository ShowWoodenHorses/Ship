using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.Animation
{
    public class SailController : MonoBehaviour
    {
        public GameObject sailUp;
        public GameObject sailDown;
        public float transitionTime = 2f;

        private Sequence anim;

        public void LowerSails()
        {
            sailDown.SetActive(true);
            sailDown.transform.localScale = Vector3.zero;

            anim = DOTween.Sequence();
            anim
                .Append(sailUp.transform.DOScale(Vector3.zero, transitionTime).SetEase(Ease.InOutSine).SetLink(sailUp))
                .Join(sailDown.transform.DOScale(Vector3.one, transitionTime).SetEase(Ease.InOutSine).SetLink(sailDown))
                .OnComplete(() => sailUp.SetActive(false));
        }

        public void RaiseSails()
        {
            sailUp.SetActive(true);
            sailUp.transform.localScale = Vector3.zero;

            anim = DOTween.Sequence();
            anim
                .Append(sailDown.transform.DOScale(Vector3.zero, transitionTime).SetEase(Ease.InOutSine).SetLink(sailDown))
                .Join(sailUp.transform.DOScale(Vector3.one, transitionTime).SetEase(Ease.InOutSine).SetLink(sailUp))
                .OnComplete(() => sailDown.SetActive(false));
        }

        private bool InAnimation() => anim != null && anim.active;

        private void OnDestroy()
        {
            if(InAnimation())
                anim.Kill();
        }
    }
}