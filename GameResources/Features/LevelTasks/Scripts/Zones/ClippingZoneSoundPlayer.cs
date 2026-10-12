namespace WinterDrift.Features.LevelTasks
{
    using DekDev.Extensions.AbstractViews;
    using UnityEngine;
    using WinterDrift.Features.Triggers;

    /// <summary>
    /// Воспроизведение звука собранной клиппинг зоны.
    /// </summary>
    public class ClippingZoneSoundPlayer : AbstractAudioSource
    {
        [SerializeField] protected ClippingZoneDisabler zoneDisabler = default;
        [SerializeField] protected ClippingZone currentZone = default;

        protected virtual void OnEnable() => zoneDisabler.onDisableClippingZone += CheckZoneCollected;

        protected virtual void OnDisable() => zoneDisabler.onDisableClippingZone -= CheckZoneCollected;

        protected virtual void PlaySound()
        {
            if (audioSource != null && audioSource.clip != null)
            {
                audioSource.Play();
            }
        }

        protected virtual void CheckZoneCollected() => PlaySound();
    }
}