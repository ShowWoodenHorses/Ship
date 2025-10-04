using DG.Tweening;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.Scripts.Sound
{
    public class MusicManager : MonoBehaviour
    {
        public static MusicManager Instance;

        [SerializeField] private AudioSource audioSourcePrefab;
        [SerializeField] private AudioMixer mainMixer;          // общий миксер
        [SerializeField] private AudioMixerGroup musicGroup;    // группа для музыки
        [SerializeField] private string musicParam = "MusicVolume"; // параметр в миксере

        private AudioSource currentAudio;
        private AudioSettingsManager audioSettingsManager;
        private const string MusicKey = "MusicVolume";

        public void Initialize(AudioSettingsManager audioSettingsManager, AudioMixer mainMixer)
        {
            if (Instance == null) Instance = this;
            else
            {
                Destroy(gameObject);
                return;
            }

            this.audioSettingsManager = audioSettingsManager;
            this.mainMixer = mainMixer;

            currentAudio = Instantiate(audioSourcePrefab, transform);
            currentAudio.loop = true;
            currentAudio.playOnAwake = false;
            currentAudio.outputAudioMixerGroup = musicGroup;
            currentAudio.volume = 1f; // всегда 1, регулировка только через AudioMixer

            PlayMusic();
        }

        public void PlayMusic(float fadeTime = 1f)
        {
            if (!currentAudio.isPlaying)
                currentAudio.Play();


            // Если первый запуск — плавный FadeIn
            if (!PlayerPrefs.HasKey(MusicKey))
            {
                FadeInMusicDOTween(0.01f, 
                    audioSettingsManager.GetDefaultMusicVolume(), 
                    audioSettingsManager.GetMusicFadeInDuration());
            }
            else
            {
                // Получаем текущий уровень
                float startVal;
                mainMixer.GetFloat(musicParam, out startVal);
                startVal = Mathf.Pow(10, startVal / 20f); // перевод dB -> 0..1

                // Фейдим через AudioMixer
                DOTween.To(() => 0.01f,
                           x => mainMixer.SetFloat(musicParam, Mathf.Log10(x) * 20f),
                           startVal,
                           fadeTime);
            }
        }

        public void StopMusic(float fadeTime = 1f)
        {
            float startVal;
            mainMixer.GetFloat(musicParam, out startVal);
            startVal = Mathf.Pow(10, startVal / 20f);

            DOTween.To(() => startVal,
                       x => mainMixer.SetFloat(musicParam, Mathf.Log10(x) * 20f),
                       0.01f,   // не ставим 0, иначе log10
                       fadeTime)
                   .OnComplete(() => currentAudio.Stop());
        }

        private void FadeInMusicDOTween(float from, float to, float duration)
        {
            DOTween.To(() => from,
                       x =>
                       {
                           mainMixer.SetFloat("MusicVolume", Mathf.Log10(x) * 20f);
                       },
                       to,
                       duration)
                   .SetUpdate(true); // работает даже при паузе
        }
    }
}
