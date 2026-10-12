namespace WinterDrift.Features.AudioManager
{
    using UnityEngine;
    using UnityEngine.Audio;
    using UnityEngine.SceneManagement;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Главный менеджер аудио в игре (синглтон)
    /// Управляет всей аудио-системой, включая музыку и звуковые эффекты
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        /// <summary>
        /// Ключ префсов громкости музыки.
        /// </summary>
        public const string MUSIC_VOLUME = "MusicVolume";

        /// <summary>
        /// Ключ префсов громкости звуков.
        /// </summary>
        public const string SFX_UI_VOLUME = "SFXAndUIVolume";

        /// <summary>
        /// Ключ префсов громкости звуков.
        /// </summary>
        public const string SOUND_VOLUME = "SFXVolume";

        /// <summary>
        /// Единственный экземпляр SoundManager
        /// </summary>
        public static SoundManager Instance { get; protected set; }

        [Header("Ссылки")]
        [SerializeField] protected AudioMixer audioMixer;
        [SerializeField] protected AudioData audioData;

        [Header("Источники звука")]
        [SerializeField] protected AudioSource musicAudioSource;
        [SerializeField] protected AudioSource uiAudioSource;
        [SerializeField] protected AudioSource sfxAudioSource;

        [Header("Параметры микшера")]
        [SerializeField] protected string musicVolumeParam = "MusicVolume";
        [SerializeField] protected string sfxVolumeParam = "SFXVolume";
        [SerializeField] protected string uiVolumeParam = "UIVolume";

        protected MusicPlayer musicPlayer;
        protected UISoundController uiSoundController;
        protected float musicVolume = 0.7f;
        protected float sfxVolume = 0.8f;
        protected MusicType currentMusicType = MusicType.None;

        #region Unity Lifecycle

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);

                LoadVolumeSettings();

                Initialize();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        protected virtual void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected virtual void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        #endregion

        #region Initialization

        protected virtual void Initialize()
        {
            CreateAndSetupComponents();
            SetupAudioMixer();
        }

        protected virtual void CreateAndSetupComponents()
        {
            musicPlayer = gameObject.AddComponent<MusicPlayer>();
            musicPlayer.Initialize(audioData, musicAudioSource);

            uiSoundController = gameObject.AddComponent<UISoundController>();
            uiSoundController.Initialize(audioData, uiAudioSource);
        }

        protected virtual void SetupAudioMixer()
        {
            if (audioMixer != null)
            {
                SetMusicVolume(musicVolume);
                SetSFXAndUIVolume(sfxVolume);

                audioMixer.SetFloat(uiVolumeParam, Mathf.Log10(sfxVolume) * 20);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Воспроизведение музыки указанного типа
        /// </summary>
        /// <param name="type">Тип музыки для воспроизведения</param>
        public virtual void PlayMusic(MusicType type)
        {
            if (currentMusicType == type)
            {
                return;
            }

            currentMusicType = type;

            switch (type)
            {
                case MusicType.Menu:
                    musicPlayer.PlayMenuMusic();
                    break;
                case MusicType.Gameplay:
                    musicPlayer.PlayGameplayMusic();
                    break;
                case MusicType.None:
                    musicPlayer.StopMusic();
                    break;
                default:
                    Debug.LogWarning($"Неизвестный тип музыки: {type}");
                    break;
            }
        }

        /// <summary>
        /// Воспроизведение звукового эффекта
        /// </summary>
        /// <param name="clip">Аудио клип для воспроизведения</param>
        public virtual void PlaySFX(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }

            if (sfxAudioSource == null)
            {
                return;
            }

            sfxAudioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Воспроизведение звука пользовательского интерфейса
        /// </summary>
        /// <param name="clip">Аудио клип для воспроизведения</param>
        public virtual void PlayUI(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }

            if (uiAudioSource == null)
            {
                return;
            }

            uiAudioSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Установка громкости музыки
        /// </summary>
        /// <param name="volume">Значение громкости от 0 до 1</param>
        public virtual void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);

            if (audioMixer != null)
            {
                float dbVolume = musicVolume > 0.0001f
                    ? Mathf.Log10(musicVolume) * 20
                    : -80f;

                audioMixer.SetFloat(musicVolumeParam, dbVolume);
            }

            if (musicPlayer != null)
            {
                musicPlayer.SetVolume(musicVolume);
            }

            PlayerPrefs.SetFloat(MUSIC_VOLUME, musicVolume);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Установка громкости звуков и UI
        /// </summary>
        /// <param name="volume">Значение громкости от 0 до 1</param>
        public virtual void SetSFXAndUIVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);

            if (audioMixer != null)
            {
                float dbVolume = sfxVolume > 0.0001f
                    ? Mathf.Log10(sfxVolume) * 20
                    : -80f;

                audioMixer.SetFloat(sfxVolumeParam, dbVolume);
                audioMixer.SetFloat(uiVolumeParam, dbVolume);
            }

            if (uiSoundController != null)
            {
                uiSoundController.SetVolume(sfxVolume);
            }

            PlayerPrefs.SetFloat(SFX_UI_VOLUME, sfxVolume);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Установка громкости только звуков без UI
        /// </summary>
        /// <param name="volume">Значение громкости от 0 до 1</param>
        public virtual void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);

            if (audioMixer != null)
            {
                float dbVolume = sfxVolume > 0.0001f
                    ? Mathf.Log10(sfxVolume) * 20
                    : -80f;

                audioMixer.SetFloat(sfxVolumeParam, dbVolume);
            }

            PlayerPrefs.SetFloat(SOUND_VOLUME, sfxVolume);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Получение текущей громкости музыки
        /// </summary>
        /// <returns>Текущая громкость музыки</returns>
        public virtual float GetMusicVolume()
        {
            return musicVolume;
        }

        /// <summary>
        /// Получение текущей громкости звуковых эффектов
        /// </summary>
        /// <returns>Текущая громкость звуковых эффектов</returns>
        public virtual float GetSFXVolume()
        {
            return sfxVolume;
        }

        #endregion

        #region Scene Management

        /// <summary>
        /// Обработчик загрузки сцены
        /// </summary>
        /// <param name="scene">Загруженная сцена</param>
        /// <param name="mode">Режим загрузки сцены</param>
        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name.Contains("Menu"))
            {
                PlayMusic(MusicType.Menu);
            }
            else if (scene.name.Contains("Level") || scene.name.Contains("Game"))
            {
                PlayMusic(MusicType.Gameplay);
            }
            else
            {
                PlayMusic(MusicType.None);
            }
        }

        #endregion

        #region Settings Persistence

        /// <summary>
        /// Загрузка настроек громкости из PlayerPrefs
        /// </summary>
        protected virtual void LoadVolumeSettings()
        {
            musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME, audioData.DefaultMusicVolume);
            sfxVolume = PlayerPrefs.GetFloat(SOUND_VOLUME, audioData.DefaultSFXVolume);

            SetMusicVolume(musicVolume);
            SetSFXAndUIVolume(sfxVolume);
        }

        /// <summary>
        /// Сохранение настроек громкости в PlayerPrefs
        /// </summary>
        public virtual void SaveVolumeSettings()
        {
            PlayerPrefs.SetFloat(MUSIC_VOLUME, musicVolume);
            PlayerPrefs.SetFloat(SOUND_VOLUME, sfxVolume);
            PlayerPrefs.Save();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Получение случайного музыкального клипа для меню
        /// </summary>
        /// <returns>Случайный аудио клип для меню</returns>
        public virtual AudioClip GetRandomMenuClip()
        {
            if (audioData.MenuMusicClips.Count > 0)
            {
                return audioData.MenuMusicClips[Random.Range(0, audioData.MenuMusicClips.Count)];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Получение случайного музыкального клипа для игрового процесса
        /// </summary>
        /// <returns>Случайный аудио клип для игрового процесса</returns>
        public virtual AudioClip GetRandomGameplayClip()
        {
            if (audioData.GameplayMusicClips.Count > 0)
            {
                return audioData.GameplayMusicClips[Random.Range(0, audioData.GameplayMusicClips.Count)];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}