namespace WinterDrift.Features.LevelTasks.Views
{
    using System.Linq;
    using DekDev.CDebug;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.PlayerFacade.Views;

    /// <summary>
    /// Total drift score value view.
    /// </summary>
    public class TotalDriftScoreView : AbstractPlayerProviderTextView
    {
        [SerializeField] protected DriftScoreModel model = default;
        protected float totalScore = 0f;

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            model = facade.Models.OfType<DriftScoreModel>().FirstOrDefault();

            if (model == null)
            {
                CDebug.LogError("DriftScoreView: DriftScoreModel not found!");
                return;
            }

            model.OnTotalScoreUpdated += UpdateTextView;

            UpdateTextView();
        }

        protected override void UpdateTextView() => textView.text = ((int)model.TotalDriftScore).ToString();

        protected override void Unsubscribe()
        {
            if (model != null)
            {
                model.OnTotalScoreUpdated -= UpdateTextView;
            }
        }
    }
}