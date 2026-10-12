namespace WinterDrift.Features.LevelTasks
{
    using DekDev.CDebug;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.Triggers;

    /// <summary>
    /// Start particle effect if current zone collected.
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class ClippingZoneCollectedEffect : MonoBehaviour
    {
        [SerializeField] private ClippingZoneModel _model = default;
        [SerializeField] private ClippingZone _currentZone = default;
        private ParticleSystem _particleEffect = default;

        private void Awake() => _particleEffect = GetComponent<ParticleSystem>();

        private void OnEnable() => _model.OnZoneCollected += CheckZoneCollected;

        private void OnDisable() => _model.OnZoneCollected -= CheckZoneCollected;

        private void CheckZoneCollected()
        {
            if (_model.IsZoneCollected(_currentZone))
            {
                _particleEffect.Play();
            }
        }
    }
}