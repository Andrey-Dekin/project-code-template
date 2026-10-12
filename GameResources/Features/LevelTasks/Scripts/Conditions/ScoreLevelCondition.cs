namespace WinterDrift.Features.LevelTasks
{
    using System.Linq;
    using DekDev.CDebug;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.SaveSystem.Levels;

    /// <summary>
    /// Score level condition.
    /// </summary>
    public class ScoreLevelCondition : BaseFacadeLevelCondition, IRatedByStars
    {
        protected DriftScoreModel model = default;

        protected override void OnFacadeInited()
        {
            base.OnFacadeInited();

            model = facade.Models.OfType<DriftScoreModel>().FirstOrDefault();

            if (model == null)
            {
                CDebug.LogError("Couldn't found model in Facade");
            }
        }

        public override void UpdateConditionMet()
        {
            if (isInProgress && model != null && model.TotalDriftScore >= targetValue)
            {
                OnConditionMet();
            }
        }

        protected override void UpdateBestScore()
        {
            if (model != null)
            {
                UpdateStarsCount((int)model.TotalDriftScore);

                levelController.SetBestScore((int)model.TotalDriftScore);
            }
        }
    }
}