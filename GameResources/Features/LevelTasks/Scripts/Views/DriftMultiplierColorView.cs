namespace WinterDrift.Features.LevelTasks.Views
{
    using System.Linq;
    using DekDev.CDebug;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.PlayerFacade.Views;

    /// <summary>
    /// Drift multiplier color view.
    /// </summary>
    public class DriftMultiplierColorView : AbstractPlayerProviderTextView
    {
        [SerializeField] protected Color minMultiplierColor = Color.green;
        [SerializeField] protected Color maxMultiplierColor = Color.red;
        protected DriftScoreModel model = default;

        protected float t = 0f;
        protected float currentMultiplier = 0f;
        protected float maxMultiplier = 0f;
        protected Color targetColor = default;

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            model = facade.Models.OfType<DriftScoreModel>().FirstOrDefault();

            if (model == null)
            {
                CDebug.LogError("DriftMultiplierColorView: DriftScoreModel not found!");
                return;
            }

            var eventInfo = typeof(DriftScoreModel).GetEvent("OnMultiplierUpdated");
            if (eventInfo == null)
            {
                CDebug.LogError("DriftMultiplierColorView: OnMultiplierUpdated event not found in DriftScoreModel!");
                return;
            }

            model.OnMultiplierUpdated += UpdateTextColor;

            UpdateTextColor();
        }

        protected void UpdateTextColor()
        {
            if (model == null) return;

            currentMultiplier = model.CurrentMultiplier;
            maxMultiplier = model.MaxTimeMultiplier;

            t = Mathf.Clamp01((currentMultiplier - 1f) / (maxMultiplier - 1f));
            
            targetColor = Color.Lerp(minMultiplierColor, maxMultiplierColor, t);

            textView.color = targetColor;
        }

        protected override void Unsubscribe()
        {
            if (model != null)
            {
                model.OnMultiplierUpdated -= UpdateTextColor;
            }
        }

        protected override void UpdateTextView()
        {
        }
    }
}