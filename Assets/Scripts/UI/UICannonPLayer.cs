using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class UICannonPLayer : MonoBehaviour
    {
        [SerializeField] private GameObject activeState;
        [SerializeField] private GameObject disableState;
        [SerializeField] private float reloadTime;

        private float currentTime;

        public void Initialize(float reloadTime)
        {
            activeState.SetActive(true);
            disableState.SetActive(false);
            this.reloadTime = reloadTime;
            currentTime = reloadTime;
        }

        private void Update()
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0)
            {
                currentTime = reloadTime;
                SetAtiveState();
            }
        }

        public void SetDisableState()
        {
            activeState.SetActive(false);
            disableState.SetActive(true);
            currentTime = reloadTime;
        }

        public void SetAtiveState()
        {
            activeState.SetActive(true);
            disableState.SetActive(false);
        }
    }
}