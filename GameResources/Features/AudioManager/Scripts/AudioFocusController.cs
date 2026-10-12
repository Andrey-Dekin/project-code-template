namespace WinterDrift.Features.AudioManager
{
    using DekDev.CDebug;
    using UnityEngine;
    using UnityEngine.Audio;

    /// <summary>
    /// Контроллер сброса громкости микшера при потере фокуса приложения.
    /// </summary>
    public class AudioFocusController : MonoBehaviour
    {
        [Header("Настройки аудио")]
        [SerializeField] protected AudioMixer audioMixer;
        [SerializeField] protected string volumeParameter = "MasterVolume";

        [Header("Настройки фокуса")]
        [SerializeField] protected bool muteOnLostFocus = true;
        [SerializeField] protected float mutedVolumeDB = -80f;

        protected float savedVolumeDB;
        protected bool isMuted = false;

        protected virtual void Start()
        {
            InitializeSavedVolume();
        }

        protected virtual void InitializeSavedVolume()
        {
            if (audioMixer != null)
            {
                if (audioMixer.GetFloat(volumeParameter, out float currentVolume))
                {
                    savedVolumeDB = currentVolume;
                }
                else
                {
                    CDebug.LogWarning($"Не удалось получить параметр {volumeParameter} из микшера");
                    savedVolumeDB = 0f;
                }
            }
        }

        protected virtual void OnApplicationFocus(bool hasFocus) => UpdateMuteStatus(!hasFocus);

        protected virtual void OnApplicationPause(bool pauseStatus) => UpdateMuteStatus(pauseStatus);

        protected virtual void UpdateMuteStatus(bool muteStatus)
        {
            if (!muteOnLostFocus) return;

            if (muteStatus)
            {
                MuteAudio();
            }
            else
            {
                RestoreAudio();
            }
        }

        /// <summary>
        /// Выключение звука
        /// </summary>
        public void MuteAudio()
        {
            if (isMuted) return;

            if (audioMixer != null)
            {
                if (audioMixer.GetFloat(volumeParameter, out float currentVolume))
                {
                    savedVolumeDB = currentVolume;
                }

                audioMixer.SetFloat(volumeParameter, mutedVolumeDB);
            }

            isMuted = true;
        }

        /// <summary>
        /// Включение звука
        /// </summary>
        public void RestoreAudio()
        {
            if (!isMuted) return;

            if (audioMixer != null)
            {
                audioMixer.SetFloat(volumeParameter, savedVolumeDB);
            }

            isMuted = false;
        }
    }
}