namespace WinterDrift.Features.PlayerFacade
{
    using System.Linq;
    using DekDev.CDebug;
    using DekDev.Extensions.Facade;
    using UnityEngine;
    using WinterDrift.Features.Triggers;

    /// <summary>
    /// Controller of passed clipping zones for player facade.
    /// </summary>
    public class ClippingZoneController : AbstractControllerFacade<PlayerFacade>
    {
        [SerializeField] protected ClippingZoneTrigger clippingZoneTrigger = default;
        protected ClippingZoneModel clippingZoneModel = default;
        protected CarController carController = default;

        public override void InitModule()
        {
            clippingZoneModel = facadeInstance.Models.OfType<ClippingZoneModel>().FirstOrDefault();
            carController = facadeInstance.Controllers.OfType<CarController>().FirstOrDefault();

            if (clippingZoneTrigger != null)
            {
                clippingZoneTrigger.onTriggerEntered += OnClippingZoneEntered;
            }
        }

        public override void Dispose()
        {
            DisposeData();

            base.Dispose();
        }

        protected void OnClippingZoneEntered()
        {
            if (clippingZoneTrigger == null || clippingZoneModel == null) return;

            ClippingZone zone = clippingZoneTrigger.CurrentZone;

            if (zone != null && carController.IsDrifting && !clippingZoneModel.IsZoneCollected(zone))
            {
                clippingZoneModel.CollectZone(zone);
            }
        }

        protected virtual void OnDisable() => DisposeData();

        protected virtual void DisposeData()
        {
            if (clippingZoneTrigger != null)
            {
                clippingZoneTrigger.onTriggerEntered -= OnClippingZoneEntered;
            }

            if (clippingZoneModel != null)
            {
                clippingZoneModel.ResetCollection();
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (clippingZoneTrigger == null)
            {
                CDebug.LogError("Clipping zone trigger field is null!");
            }
        }
#endif
    }
}