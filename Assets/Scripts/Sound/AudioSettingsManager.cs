using UnityEngine;
using UnityEngine.Audio;
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
        [SerializeField] private float defaultMusicVolume = 0.3f;
        [Range(0.01f, 1f)]
        [SerializeField] private float defaultSFXVolume = 0.3f;

        [Tooltip("Время плавного нарастания музыки при первом запуске")]
        [SerializeField] private float musicFadeInDuration = 3f;

        [Header("Mute")]
        [SerializeField] private Button muteButton;

        private float currentValueMusic = 0f;
        private float currentValueSFX = 0f;

        private void OnEnable()
        {
            GlobalAudioManager.Instance.OnToggleMute += MuteSound;
        }

        private void OnDisable()
        {
            GlobalAudioManager.Instance.OnToggleMute -= MuteSound;
        }


        public void Initialize()
        {
            // Проверяем сохранённые значения
            float savedMusic = PlayerPrefs.HasKey(MusicKey) ? PlayerPrefs.GetFloat(MusicKey) : defaultMusicVolume;
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

            if (musicSlider.value <= 0.05f)
                MusicManager.Instance.PauseMusic();

            else if (musicSlider.value > 0.05f)
                MusicManager.Instance.ContinueMusic();

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

        private void MuteSound(bool isMute)
        {
            if (isMute)
            {
                float minValue = 0.01f;
                currentValueMusic = musicSlider.value;
                currentValueSFX = soundSlider.value;

                musicSlider.SetValueWithoutNotify(minValue);
                soundSlider.SetValueWithoutNotify(minValue);

                PlayerPrefs.SetFloat(MusicKey, minValue);
                PlayerPrefs.SetFloat(SFXKey, minValue);

                PlayerPrefs.Save();

                if (musicSlider.value <= 0.05f)
                    MusicManager.Instance.PauseMusic();

            }
            else
            {
                musicSlider.value = currentValueMusic;
                soundSlider.value = currentValueSFX;

                musicSlider.SetValueWithoutNotify(musicSlider.value);
                soundSlider.SetValueWithoutNotify(soundSlider.value);

                PlayerPrefs.SetFloat(MusicKey, musicSlider.value);
                PlayerPrefs.SetFloat(SFXKey, soundSlider.value);

                PlayerPrefs.Save();

                if (musicSlider.value > 0.05f)
                    MusicManager.Instance.ContinueMusic();
            }
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
