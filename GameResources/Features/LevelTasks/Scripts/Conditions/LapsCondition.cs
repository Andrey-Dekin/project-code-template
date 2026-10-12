namespace WinterDrift.Features.LevelTasks
{
    using System.Linq;
    using DekDev.CDebug;
    using WinterDrift.Features.PlayerFacade;

    /// <summary>
    /// Condition of the number of laps.
    /// </summary>
    public class LapsCondition : BaseFacadeLevelCondition
    {
        protected LapLineController controller = default;

        protected override void OnFacadeInited()
        {
            base.OnFacadeInited();

            controller = facade.Controllers.OfType<LapLineController>().FirstOrDefault();

            if (controller != null)
            {
                controller.onLineCrossed += UpdateConditionMet;
            }

            if (controller == null)
            {
                CDebug.LogError("Couldn't found model in Facade");
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            UnsubscribeData();
        }

        public override void UpdateConditionMet()
        {
            if (isInProgress && controller != null && controller.LapsCount > targetValue)
            {
                OnConditionMet();

                UnsubscribeData();

                levelController.Finish();
            }
        }

        protected override void UpdateBestScore() { }

        protected virtual void UnsubscribeData()
        {
            if (controller != null)
            {
                controller.onLineCrossed -= UpdateConditionMet;
            }
        }
    }
}