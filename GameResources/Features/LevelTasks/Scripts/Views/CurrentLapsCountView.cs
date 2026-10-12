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
    public class CurrentLapsCountView : AbstractPlayerProviderTextView
    {
        protected LapLineController controller = default;
        protected int currentLapsCount = 0;

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            controller = facade.Controllers.OfType<LapLineController>().FirstOrDefault();

            if (controller == null)
            {
                CDebug.LogError("LapLineController not found!");
                return;
            }

            controller.onLineCrossed += UpdateTextView;

            UpdateTextView();
        }

        protected override void UpdateTextView()
        {
            currentLapsCount = controller.LapsCount - 1;
            textView.text = currentLapsCount > 0 ? currentLapsCount.ToString() : "0";
        }

        protected override void Unsubscribe()
        {
            if (controller != null)
            {
                controller.onLineCrossed -= UpdateTextView;
            }
        }
    }
}