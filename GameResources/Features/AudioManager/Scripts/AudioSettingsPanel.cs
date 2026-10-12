namespace WinterDrift.Features.AudioManager
{
    using UnityEngine;
    using UnityEngine.UI;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Панель настроек аудио с элементами управления громкостью
    /// </summary>
    public class AudioSettingsPanel : MonoBehaviour
    {
        [Header("Ссылки на элементы пользовательского интерфейса")]
        [SerializeField] protected Slider musicSlider;
        [SerializeField] protected Slider sfxSlider;
        [SerializeField] protected Text musicPercentText;
        [SerializeField] protected Text sfxPercentText;

        [Header("Настройки отображения")]
        [SerializeField] protected bool showPercentage = true;

        protected virtual void Start()
        {
            Initialize();
        }

        protected virtual void OnDisable() => SaveSettings();

        protected virtual void OnDestroy()
        {
            Unsubscribe();
        }

        /// <summary>
        /// Инициализация панели настроек аудио
        /// </summary>
        public virtual void Initialize()
        {
            LoadSettings();
            Subscribe();
            UpdateVolumeTexts();
        }

        /// <summary>
        /// Обработчик изменения значения слайдера музыки
        /// </summary>
        /// <param name="value">Новое значение громкости</param>
        public virtual void OnMusicSliderChanged(float value)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetMusicVolume(value);
            }

            UpdateVolumeTexts();
        }

        /// <summary>
        /// Обработчик изменения значения слайдера звуковых эффектов
        /// </summary>
        /// <param name="value">Новое значение громкости</param>
        public virtual void OnSFXSliderChanged(float value)
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetSFXAndUIVolume(value);
            }

            UpdateVolumeTexts();
        }

        /// <summary>
        /// Обновление текстовых полей с текущими значениями громкости
        /// </summary>
        public virtual void UpdateVolumeTexts()
        {
            UpdateMusicVolumeText();
            UpdateSFXVolumeText();
        }

        /// <summary>
        /// Сохранение настроек громкости
        /// </summary>
        public virtual void SaveSettings()
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SaveVolumeSettings();
            }
        }

        /// <summary>
        /// Загрузка настроек громкости
        /// </summary>
        public virtual void LoadSettings()
        {
            if (SoundManager.Instance == null)
            {
                return;
            }

            float musicVolume = PlayerPrefs.GetFloat(SoundManager.MUSIC_VOLUME, musicSlider.value);
            float sfxVolume = PlayerPrefs.GetFloat(SoundManager.SOUND_VOLUME, sfxSlider.value);

            if (musicSlider != null)
            {
                musicSlider.value = musicVolume;
            }

            if (sfxSlider != null)
            {
                sfxSlider.value = sfxVolume;
            }

            SoundManager.Instance.SetMusicVolume(musicVolume);
            SoundManager.Instance.SetSFXAndUIVolume(sfxVolume);
        }

        protected virtual void UpdateMusicVolumeText()
        {
            if (musicPercentText != null && musicSlider != null)
            {
                if (showPercentage)
                {
                    musicPercentText.text = $"{Mathf.RoundToInt(musicSlider.value * 100)}%";
                }
                else
                {
                    musicPercentText.text = $"{musicSlider.value:F2}";
                }
            }
        }

        protected virtual void UpdateSFXVolumeText()
        {
            if (sfxPercentText != null && sfxSlider != null)
            {
                if (showPercentage)
                {
                    sfxPercentText.text = $"{Mathf.RoundToInt(sfxSlider.value * 100)}%";
                }
                else
                {
                    sfxPercentText.text = $"{sfxSlider.value:F2}";
                }
            }
        }

        protected virtual void Subscribe()
        {
            if (musicSlider != null)
            {
                musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
            }

            if (sfxSlider != null)
            {
                sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
            }
        }

        protected virtual void Unsubscribe()
        {
            if (musicSlider != null)
            {
                musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
            }

            if (sfxSlider != null)
            {
                sfxSlider.onValueChanged.RemoveListener(OnSFXSliderChanged);
            }
        }
    }
}