namespace WinterDrift.Features.LevelTasks
{
    using System;
    using System.Linq;
    using DekDev.CDebug;
    using DekDev.Extensions.Timers;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;

    /// <summary>
    /// Timer level condition.
    /// </summary>
    public class TimeLevelCondition : BaseFacadeLevelCondition
    {
        protected const float START_SPEED = 1f;

        /// <summary>
        /// Timer for views.
        /// </summary>
        public CoroutineTimerExtension Timer => timer;

        protected CarController controller = default;
        protected CoroutineTimerExtension timer = default;

        protected virtual void Awake()
        {
            timer = new CoroutineTimerExtension(this);
            timer.onFinished += UpdateConditionMet;
        }

        protected override void SetDefaultConditionStatus() => isConditionMet = true;

        protected override void OnFacadeInited()
        {
            base.OnFacadeInited();

            controller = facade.Controllers.OfType<CarController>().FirstOrDefault();

            if (controller != null)
            {
                controller.onSpeedChanged += CheckSpeedToStart;
            }
            else
            {
                CDebug.LogError("Couldn't found controller in Facade");
            }

            levelController.onWin += OnLevelCompleted;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            UnsubscribeAll();

            timer.Dispose();
        }

        protected void CheckSpeedToStart()
        {
            if (controller.CurrentSpeed >= START_SPEED)
            {
                controller.onSpeedChanged -= CheckSpeedToStart;

                timer.StartCountdown(TimeSpan.FromSeconds(targetValue));
            }
        }

        protected void OnLevelCompleted()
        {
            UnsubscribeAll();

            timer.Stop();

            OnConditionMet();
        }

        public override void UpdateConditionMet()
        {
            if (isInProgress && timer.CurrentTime.TotalSeconds <= 0)
            {
                UnsubscribeAll();

                OnConditionFail();

                levelController.Finish();
            }
        }

        protected override void UpdateBestScore()
        {
            if (targetValue != 0 && timer != null && targetValue > timer.CurrentTime.TotalSeconds)
            {
                levelController.SetBestTime(TimeSpan.FromSeconds(targetValue).Subtract(timer.CurrentTime));
            }
            else
            {
                CDebug.LogError("Can't set best time result - timer is null");
            }
        }

        protected void UnsubscribeAll()
        {
            if (levelController != null)
            {
                levelController.onWin -= OnLevelCompleted;
            }

            if (controller != null)
            {
                controller.onSpeedChanged -= CheckSpeedToStart;
            }

            timer.onFinished -= UpdateConditionMet;
        }
    }
}