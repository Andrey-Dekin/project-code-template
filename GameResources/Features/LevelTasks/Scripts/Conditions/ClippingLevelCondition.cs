namespace WinterDrift.Features.LevelTasks
{
    using System.Linq;
    using DekDev.CDebug;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.SaveSystem.Levels;

    /// <summary>
    /// Clipping zone collected level condition.
    /// </summary>
    public class ClippingLevelCondition : BaseFacadeLevelCondition, IRatedByStars
    {
        protected ClippingZoneModel model = default;

        protected override void OnFacadeInited()
        {
            base.OnFacadeInited();

            model = facade.Models.OfType<ClippingZoneModel>().FirstOrDefault();

            if (model == null)
            {
                CDebug.LogError("Couldn't found model in Facade");
            }
        }

        public override void UpdateConditionMet()
        {
            if (isInProgress && model != null && model.CollectedZonesCount >= targetValue)
            {
                OnConditionMet();
            }
        }

        protected override void UpdateBestScore()
        {
            if (model != null)
            {
                UpdateStarsCount(model.CollectedZonesCount);

                levelController.SetBestClippingZoneScore(model.CollectedZonesCount);
            }
        }
    }
}