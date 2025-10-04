using UnityEngine;
using UnityEngine.Audio;
using DG.Tweening;
using UnityEngine.UI;

namespace Assets.Scripts.Sound
{
    public class AudioSettingsManager : MonoBehaviour
    {
        public AudioMixer mainMixer;

        private const string MusicKey = "MusicVolume";
        private const string SFXKey = "SFXVolume";

        [SerializeField] private Slider musicSlider;
        [SerializeField] private Slider soundSlider;

        [Range(0.01f, 1f)]
        [SerializeField] private float defaultMusicVolume = 0.8f;
        [Range(0.01f, 1f)]
        [SerializeField] private float defaultSFXVolume = 0.8f;

        [Tooltip("Время плавного нарастания музыки при первом запуске")]
        [SerializeField] private float musicFadeInDuration = 3f;

        [Header("Mute")]
        [SerializeField] private Button muteButton;


        public void Initialize()
        {
            // Проверяем сохранённые значения
            float savedMusic = PlayerPrefs.HasKey(MusicKey) ? PlayerPrefs.GetFloat(MusicKey) : 0.01f;
            float savedSFX = PlayerPrefs.HasKey(SFXKey) ? PlayerPrefs.GetFloat(SFXKey) : defaultSFXVolume;

            // Сначала выставляем громкость в миксер (без вызова событий)
            mainMixer.SetFloat("MusicVolume", Mathf.Log10(savedMusic) * 20f);
            mainMixer.SetFloat("SFXVolume", Mathf.Log10(savedSFX) * 20f);

            // Обновляем UI без вызова onValueChanged
            musicSlider.SetValueWithoutNotify(savedMusic);
            soundSlider.SetValueWithoutNotify(savedSFX);

            // Теперь подписываемся на события
            musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            soundSlider.onValueChanged.AddListener(OnSFXSliderChanged);
            muteButton.onClick.AddListener(() => Mute());
        }


        public void SetMusicVolume(float value)
        {
            mainMixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20f);

            PlayerPrefs.SetFloat(MusicKey, value);
            PlayerPrefs.Save();
        }

        public void SetSFXVolume(float value)
        {
            mainMixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20f);

            PlayerPrefs.SetFloat(SFXKey, value);
            PlayerPrefs.Save();
        }

        public float GetDefaultMusicVolume()
        {
            return defaultMusicVolume;
        }

        public float GetDefaultSFXVolume()
        {
            return defaultSFXVolume;
        }

        public float GetMusicFadeInDuration()
        {
            return musicFadeInDuration;
        }

        private void OnMusicSliderChanged(float value)
        {
            SetMusicVolume(value);
        }

        private void OnSFXSliderChanged(float value)
        {
            SetSFXVolume(value);
        }

        private void Mute()
        {
            GlobalAudioManager.Instance?.ToggleUserMute();
        }

    }
}
