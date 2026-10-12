namespace WinterDrift.Features.AudioManager
{
    using System.Collections.Generic;

    using UnityEngine;

    /// <summary>
    /// Контроллер для управления звуками пользовательского интерфейса
    /// </summary>
    public class UISoundController : MonoBehaviour
    {
        protected AudioData audioData;
        protected AudioSource audioSource;
        
        /// <summary>
        /// Инициализация компонента UISoundController
        /// </summary>
        /// <param name="data">Данные аудио</param>
        /// <param name="source">Источник аудио</param>
        public virtual void Initialize(AudioData data, AudioSource source)
        {
            audioData = data;
            audioSource = source;
            
            if (audioSource != null)
            {
                audioSource.loop = false;
                audioSource.playOnAwake = false;
            }
        }
        
        /// <summary>
        /// Воспроизведение звука нажатия на кнопку
        /// </summary>
        public virtual void PlayButtonClick()
        {
            if (audioData == null)
            {
                return;
            }
            
            if (audioData.UIButtonClips.Count == 0)
            {
                return;
            }
            
            AudioClip clip = GetRandomClip(audioData.UIButtonClips);
            PlayOneShot(clip);
        }
        
        /// <summary>
        /// Воспроизведение звука наведения на кнопку
        /// </summary>
        public virtual void PlayButtonHover()
        {
            if (audioData == null)
            {
                return;
            }
            
            if (audioData.UIHoverClips.Count == 0)
            {
                return;
            }
            
            AudioClip clip = GetRandomClip(audioData.UIHoverClips);
            PlayOneShot(clip);
        }
        
        /// <summary>
        /// Воспроизведение звука уведомления
        /// </summary>
        public virtual void PlayNotification()
        {
            if (audioData == null)
            {
                return;
            }
            
            if (audioData.UINotificationClips.Count == 0)
            {
                return;
            }
            
            AudioClip clip = GetRandomClip(audioData.UINotificationClips);
            PlayOneShot(clip);
        }
        
        /// <summary>
        /// Установка громкости звуков пользовательского интерфейса
        /// </summary>
        /// <param name="volume">Значение громкости от 0 до 1</param>
        public virtual void SetVolume(float volume)
        {
            if (audioSource != null)
            {
                audioSource.volume = Mathf.Clamp01(volume);
            }
        }
        
        protected virtual AudioClip GetRandomClip(List<AudioClip> clips)
        {
            if (clips == null)
            {
                return null;
            }
            
            if (clips.Count == 0)
            {
                return null;
            }
            
            return clips[Random.Range(0, clips.Count)];
        }
        
        protected virtual void PlayOneShot(AudioClip clip)
        {
            if (clip == null)
            {
                return;
            }
            
            if (audioSource == null)
            {
                return;
            }
            
            audioSource.PlayOneShot(clip);
        }
    }
}