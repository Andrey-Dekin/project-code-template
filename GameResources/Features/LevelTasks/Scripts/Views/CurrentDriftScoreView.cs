namespace WinterDrift.Features.LevelTasks.Views
{
    using System.Linq;
    using DekDev.CDebug;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.PlayerFacade.Views;

    /// <summary>
    /// Current drift score value view.
    /// </summary>
    public class CurrentDriftScoreView : AbstractPlayerProviderTextView
    {
        protected DriftScoreModel model = default;
        protected float currentScore = 0f;

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            model = facade.Models.OfType<DriftScoreModel>().FirstOrDefault();

            if (model == null)
            {
                CDebug.LogError("DriftScoreView: DriftScoreModel not found!");
                return;
            }

            model.OnDriftScoreUpdated += UpdateTextView;

            UpdateTextView();
        }

        protected override void UpdateTextView() => textView.text = ((int)model.CurrentDriftScore).ToString();

        protected override void Unsubscribe()
        {
            if (model != null)
            {
                model.OnDriftScoreUpdated -= UpdateTextView;
            }
        }
    }
}