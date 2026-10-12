namespace WinterDrift.Features.SaveSystem.Levels
{
    using System.Collections.Generic;
    using System.Linq;
    using DekDev.CDebug;
    using UnityEngine;
    using WinterDrift.Features.LevelTasks.Views;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.Triggers;

    /// <summary>
    /// Enable clipping zones for next one lap, after crossing finish line.
    /// </summary>
    public sealed class ClippingZoneEnabler : AbstractLevelView
    {
        [SerializeField] private ClippingZoneModel _clippingZoneModel = default;
        private List<ClippingZone> _clippingZones = new List<ClippingZone>();
        private bool _isActive = false;
        private bool _isLevelTypeValidated = false;

        private void FindClippingZones()
        {
            if (!_isLevelTypeValidated && levelController != null && levelController.CurrentLevel != null)
            {
                _isActive = levelController.CurrentLevel.Conditions.Find(x => x.ConditionType == LevelType.ClippingMode) != null;

                _isLevelTypeValidated = true;
            }

            if (_clippingZones.Count < 1 && _isActive)
            {
                _clippingZones = FindObjectsOfType<ClippingZone>().ToList();
            }
        }

        protected override void UpdateView()
        {
            FindClippingZones();

            _clippingZones.ForEach(x => x.gameObject.SetActive(true));
            _clippingZoneModel.ClearAllCollectedZones();

            CDebug.Log("All clipping zones reseted");
        }
    }
}