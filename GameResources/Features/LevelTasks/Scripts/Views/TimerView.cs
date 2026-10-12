namespace WinterDrift.Features.LevelTasks.Views
{
    using System;
    using System.Linq;
    using DekDev.Extensions.Timers;
    using WinterDrift.Features.SaveSystem.Levels;
    using UnityEngine;

    /// <summary>
    /// Level timer view.
    /// </summary>
    public class TimerView : AbstractLevelTextView
    {
        protected const LevelType CURRENT_TYPE = LevelType.TimeMode;
        protected const string FORMAT = "{0:00}:{1:00}";

        [SerializeField] protected bool isCountdownMode = true;
        protected CoroutineTimerExtension levelTimer = default;
        protected bool isFinished = false;

        protected override void OnLevelInited()
        {
            base.OnLevelInited();

            if (levelController != null)
            {
                TimeLevelCondition timerCondition = levelController.Conditions.OfType<TimeLevelCondition>().FirstOrDefault();

                if (timerCondition != null)
                {
                    levelTimer = timerCondition.Timer;

                    levelTimer.onSecondPassed += UpdateView;
                    levelTimer.onFinished += OnTimerFinished;
                }
            }
        }

        protected override void OnDisable()
        {
            Unsubscribe();

            base.OnDisable();
        }

        protected void OnTimerFinished()
        {
            isFinished = true;

            Unsubscribe();

            UpdateView();
        }

        protected override string GetValueString()
        {
            int targetSeconds = GetTargetConditionValue(CURRENT_TYPE);

            if (isFinished || levelTimer != null && levelTimer.IsRunning)
            {
                if (isCountdownMode)
                {
                    return GetFormatedTime(levelTimer.CurrentTime);
                }
                else
                {
                    int remainingSeconds = (int)Math.Max(0, Math.Floor(targetSeconds - levelTimer.CurrentTime.TotalSeconds));
                    return GetFormatedTime(remainingSeconds);
                }
            }

            if (currentLevel != null && currentLevel != null && LevelTypeMatchCheck(CURRENT_TYPE))
            {
                return GetFormatedTime(isCountdownMode ? targetSeconds : 0);
            }

            return string.Empty;
        }

        protected string GetFormatedTime(int totalSeconds)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(totalSeconds);

            return GetFormatedTime(timeSpan);
        }

        protected string GetFormatedTime(TimeSpan time) => string.Format(FORMAT, time.Minutes, time.Seconds);

        protected void Unsubscribe()
        {
            if (levelTimer != null)
            {
                levelTimer.onSecondPassed -= UpdateView;
                levelTimer.onFinished -= OnTimerFinished;
            }
        }
    }
}