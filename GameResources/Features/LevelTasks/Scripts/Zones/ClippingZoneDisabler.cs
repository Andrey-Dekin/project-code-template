namespace WinterDrift.Features.LevelTasks
{
    using System;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.Triggers;

    /// <summary>
    /// Disable current clipping zone object if was collected.
    /// </summary>
    [RequireComponent(typeof(ClippingZone))]
    public sealed class ClippingZoneDisabler : MonoBehaviour
    {
        /// <summary>
        /// Before disable event.
        /// </summary>
        public event Action onDisableClippingZone = delegate { };

        [SerializeField] private ClippingZoneModel _model = default;
        private ClippingZone _currentZone = default;

        private void Awake() => _currentZone = GetComponent<ClippingZone>();

        private void OnEnable() => _model.OnZoneCollected += CheckZoneCollected;

        private void OnDisable() => _model.OnZoneCollected -= CheckZoneCollected;

        private void CheckZoneCollected()
        {
            if (_model.IsZoneCollected(_currentZone))
            {
                onDisableClippingZone();
                
                DisableZone();
            }
        }
        private void DisableZone() => gameObject.SetActive(false);
    }
}