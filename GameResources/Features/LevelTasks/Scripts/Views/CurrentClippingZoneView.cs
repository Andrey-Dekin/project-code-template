namespace WinterDrift.Features.LevelTasks.Views
{
    using System.Linq;
    using DekDev.CDebug;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.PlayerFacade.Views;

    /// <summary>
    /// Current clipping zone collected value view.
    /// </summary>
    public class CurrentClippingZoneView : AbstractPlayerProviderTextView
    {
        protected ClippingZoneModel model = default;

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            model = facade.Models.OfType<ClippingZoneModel>().FirstOrDefault();

            if (model == null)
            {
                CDebug.LogError("ClippingZoneModel not found!");
                return;
            }

            model.OnZonesCountUpdated += UpdateTextView;

            UpdateTextView();
        }

        protected override void UpdateTextView() => textView.text = model.CollectedZonesCount.ToString();

        protected override void Unsubscribe()
        {
            if (model != null)
            {
                model.OnZonesCountUpdated -= UpdateTextView;
            }
        }
    }
}