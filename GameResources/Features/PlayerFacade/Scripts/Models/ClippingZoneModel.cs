namespace WinterDrift.Features.PlayerFacade
{
    using System;
    using System.Collections.Generic;
    using DekDev.Extensions.Facade;
    using UnityEngine;
    using WinterDrift.Features.Triggers;

    /// <summary>
    /// Model for collected clipping zones.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(ClippingZoneModel), menuName = "WinterDrift/Features/PlayerFacade/Models/" + nameof(ClippingZoneModel))]
    public class ClippingZoneModel : AbstractModelFacade<PlayerFacade>
    {
        public event Action OnZonesCountUpdated = delegate { };
        public event Action OnZoneCollected = delegate { };
        public event Action OnAllZonesReset = delegate { };

        public int CollectedZonesCount { get; private set; }
        public List<ClippingZone> CollectedZones { get; private set; } = new List<ClippingZone>();

        /// <summary>
        /// Add zone to the collection.
        /// </summary>
        /// <param name="zone"></param>
        public void CollectZone(ClippingZone zone)
        {
            if (zone == null || IsZoneCollected(zone)) return;

            CollectedZones.Add(zone);
            CollectedZonesCount++;

            OnZoneCollected();
            OnZonesCountUpdated();
        }

        /// <summary>
        /// Reset clipping zone collection.
        /// </summary>
        public void ResetCollection()
        {
            CollectedZones.Clear();
            CollectedZonesCount = 0;

            OnAllZonesReset();
            OnZonesCountUpdated();
        }

        /// <summary>
        /// Check if zone already collected.
        /// </summary>
        /// <param name="zone"></param>
        /// <returns></returns>
        public bool IsZoneCollected(ClippingZone zone) => CollectedZones.Contains(zone);

        /// <summary>
        /// Clear all collected zones in list, without reset count.
        /// </summary>
        public void ClearAllCollectedZones() => CollectedZones.Clear();
    }
}