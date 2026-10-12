namespace WinterDrift.Features.AudioManager
{
    using UnityEngine;

    /// <summary>
    /// Активатор снепшота UI окна.
    /// </summary>
    public sealed class WindowSoundVolumeChanger : MonoBehaviour
    {
        [SerializeField, Range(0f, 1f)] private float _targetSoundVolume = 0f;
        private float _initialVolume = 0f;

        private void OnEnable()
        {
            if (SoundManager.Instance != null)
            {
                _initialVolume = SoundManager.Instance.GetSFXVolume();

                SoundManager.Instance.SetSFXVolume(_targetSoundVolume);
            }
        }

        private void OnDisable()
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.SetSFXVolume(_initialVolume);
            }
        }
    }
}