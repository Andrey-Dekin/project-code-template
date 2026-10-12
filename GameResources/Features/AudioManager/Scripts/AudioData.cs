namespace WinterDrift.Features.AudioManager
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// ScriptableObject для хранения всех аудио-ресурсов игры
    /// </summary>
    [CreateAssetMenu(fileName = "AudioData", menuName = "WinterDrift/Audio/Audio Data")]
    public class AudioData : ScriptableObject
    {
        /// <summary>
        /// Массив музыкальных клипов для меню
        /// </summary>
        public List<AudioClip> MenuMusicClips => menuMusicClips;

        /// <summary>
        /// Массив музыкальных клипов для игрового процесса
        /// </summary>
        public List<AudioClip> GameplayMusicClips => gameplayMusicClips;

        /// <summary>
        /// Массив звуков для кнопок пользовательского интерфейса
        /// </summary>
        public List<AudioClip> UIButtonClips => uiButtonClips;

        /// <summary>
        /// Массив звуков для наведения на элементы пользовательского интерфейса
        /// </summary>
        public List<AudioClip> UIHoverClips => uiHoverClips;

        /// <summary>
        /// Массив звуков для уведомлений пользовательского интерфейса
        /// </summary>
        public List<AudioClip> UINotificationClips => uiNotificationClips;

        /// <summary>
        /// Громкость музыки по умолчанию
        /// </summary>
        public float DefaultMusicVolume => defaultMusicVolume;

        /// <summary>
        /// Громкость звуковых эффектов по умолчанию
        /// </summary>
        public float DefaultSFXVolume => defaultSFXVolume;

        [Header("Музыкальные клипы")]
        [SerializeField] protected List<AudioClip> menuMusicClips = new List<AudioClip>();
        [SerializeField] protected List<AudioClip> gameplayMusicClips = new List<AudioClip>();

        [Header("Звуки пользовательского интерфейса")]
        [SerializeField] protected List<AudioClip> uiButtonClips = new List<AudioClip>();
        [SerializeField] protected List<AudioClip> uiHoverClips = new List<AudioClip>();
        [SerializeField] protected List<AudioClip> uiNotificationClips = new List<AudioClip>();

        [Header("Громкость по умолчанию")]
        [SerializeField][Range(0f, 1f)] protected float defaultMusicVolume = 0.6f;
        [SerializeField][Range(0f, 1f)] protected float defaultSFXVolume = 0.8f;
    }
}