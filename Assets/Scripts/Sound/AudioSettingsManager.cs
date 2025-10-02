using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;

namespace Assets.Scripts.Sound
{
    public class AudioSettingsManager : MonoBehaviour
    {
        public AudioMixer mainMixer;

        private const string MusicKey = "MusicVolume";
        private const string SFXKey = "SFXVolume";

        [Range(0.01f, 1f)]
        public float defaultMusicVolume = 0.8f;
        [Range(0.01f, 1f)]
        public float defaultSFXVolume = 0.8f;

        [Tooltip("Время плавного нарастания музыки при первом запуске")]
        public float musicFadeInDuration = 3f;

        public void Initialize()
        {
            // Проверяем сохранённые значения
            float savedMusic = PlayerPrefs.HasKey(MusicKey) ? PlayerPrefs.GetFloat(MusicKey) : 0.01f;
            float savedSFX = PlayerPrefs.HasKey(SFXKey) ? PlayerPrefs.GetFloat(SFXKey) : defaultSFXVolume;

            // Сначала выставляем минимальную громкость
            SetMusicVolume(savedMusic, instant: true);
            SetSFXVolume(savedSFX, instant: true);

            // Если первый запуск — делаем плавный FadeIn до defaultMusicVolume
            if (!PlayerPrefs.HasKey(MusicKey))
            {
                FadeInMusicDOTween(0.01f, defaultMusicVolume, musicFadeInDuration);
            }
        }

        public void SetMusicVolume(float value, bool instant = false)
        {
            value = Mathf.Clamp(value, 0.01f, 1f);
            if (instant)
                mainMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);

            PlayerPrefs.SetFloat(MusicKey, value);
        }

        public void SetSFXVolume(float value, bool instant = false)
        {
            value = Mathf.Clamp(value, 0.01f, 1f);
            if (instant)
                mainMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);

            PlayerPrefs.SetFloat(SFXKey, value);
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
