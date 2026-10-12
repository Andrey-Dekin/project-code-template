namespace WinterDrift.Features.LevelTasks
{
    using System;
    using System.Linq;
    using DekDev.CDebug;
    using DekDev.Extensions.Timers;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;

    /// <summary>
    /// Minimal speed level condition.
    /// </summary>
    public class MinSpeedCondition : BaseFacadeLevelCondition
    {
        /// <summary>
        /// Timer for views.
        /// </summary>
        public CoroutineTimerExtension Timer => timer;

        [SerializeField, Min(0f)] protected float countdownTimeSeconds = 3f;
        protected CarController controller = default;
        protected CoroutineTimerExtension timer = default;
        protected bool hasReachedMinSpeed = false;

        protected virtual void Awake()
        {
            timer = new CoroutineTimerExtension(this);
            timer.onFinished += OnTimerEnded;
        }

        protected override void SetDefaultConditionStatus() => isConditionMet = true;

        protected override void OnFacadeInited()
        {
            base.OnFacadeInited();

            controller = facade.Controllers.OfType<CarController>().FirstOrDefault();

            if (controller != null)
            {
                controller.onSpeedChanged += OnSpeedChanged;
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

        protected void OnSpeedChanged()
        {
            if (!hasReachedMinSpeed)
            {
                if (controller.CurrentSpeed > targetValue)
                {
                    hasReachedMinSpeed = true;
                }

                return;
            }

            if (controller.CurrentSpeed <= targetValue)
            {
                if (!timer.IsRunning)
                {
                    timer.StartCountdown(TimeSpan.FromSeconds(countdownTimeSeconds));
                }
            }
            else
            {
                if (timer.IsRunning)
                {
                    timer.Stop();
                }
            }
        }

        protected void OnLevelCompleted()
        {
            UnsubscribeAll();

            timer.Stop();

            OnConditionMet();
        }

        protected void OnTimerEnded()
        {
            if (isInProgress)
            {
                UnsubscribeAll();

                OnConditionFail();

                levelController.Finish();
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
                controller.onSpeedChanged -= OnSpeedChanged;
            }

            timer.onFinished -= OnTimerEnded;
        }
    }
}