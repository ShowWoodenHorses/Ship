using Assets.Scripts.Sound;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Animation
{
    [RequireComponent(typeof(Button))]
    public class ButtonController : MonoBehaviour
    {
        [SerializeField] private AudioSource soundClickPrefab;

        private void OnEnable()
        {
            GetComponent<Button>().onClick.AddListener(PlaySound);
        }

        private void OnDisable()
        {
            GetComponent<Button>().onClick?.RemoveListener(PlaySound);
        }

        private void PlaySound()
        {
            SoundPoolManager.Instance.PlaySound(soundClickPrefab);
        }
    }
}