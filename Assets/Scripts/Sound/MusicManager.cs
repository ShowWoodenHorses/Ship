using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Sound
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;
        [SerializeField] private AudioSource audioSourcePrefab;

        [SerializeField] private AudioMixerGroup musicGroup;

        private AudioSource currentAudio;

        public void Initialize()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);


            AudioSource audio = Instantiate(audioSourcePrefab, transform);
            currentAudio = audio;

            currentAudio.gameObject.SetActive(true);
            currentAudio.GetComponent<AudioSource>().enabled = true;

            currentAudio.loop = true;
            currentAudio.playOnAwake = false;
            currentAudio.outputAudioMixerGroup = musicGroup;
            PlayMusic();
        }

        public void PlayMusic(float fadeTime = 1f)
        {
            if (currentAudio.isPlaying)
            {
                currentAudio.DOFade(0, fadeTime).OnComplete(() =>
                {
                    currentAudio.Play();
                    currentAudio.DOFade(1, fadeTime);
                });
            }
            else
            {
                currentAudio.volume = 0;
                currentAudio.Play();
                currentAudio.DOFade(1, fadeTime);
            }
        }

        public void StopMusic(float fadeTime = 1f)
        {
            currentAudio.DOFade(0, fadeTime).OnComplete(() => currentAudio.Stop());
        }
    }
}