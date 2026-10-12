namespace WinterDrift.Features.LevelTasks
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DekDev.CDebug;
    using UnityEngine;
    using WinterDrift.Features.SaveSystem.Levels;

    /// <summary>
    /// Abstract class for checking whether the conditions of the mode are met.
    /// </summary>
    public abstract class AbstractLevelCondition : MonoBehaviour
    {
        protected int MAX_STARS_COUNT = 3;

        public event Action onConditionStatusChanged = delegate { };

        public int StarsForCondition { get; protected set; }
        public bool IsInProgress => isInProgress;
        public bool IsConditionMet => isConditionMet;

        [Header("Target values ​​are updated automatically")]
        [SerializeField] protected LevelType levelType = LevelType.ScoreMode;
        [SerializeField] protected int targetValue = 0;

        protected LevelController levelController = default;
        protected bool isInProgress = false;
        protected bool isConditionMet = false;
        protected LevelConditionConfig conditionConfig = default;

        /// <summary>
        /// Initialization from LevelController.
        /// </summary>
        /// <param name="controller"></param>
        public virtual void Initialize(LevelController controller)
        {
            SetDefaultConditionStatus();

            isInProgress = false;

            levelController = controller;

            if (levelController != null)
            {
                targetValue = 0;

                conditionConfig = levelController.CurrentLevel.Conditions.Find(x => x.ConditionType == levelType);

                if (conditionConfig != null)
                {
                    targetValue = conditionConfig.TargetValue;

                    if (conditionConfig.StarTargets.Count == MAX_STARS_COUNT && conditionConfig.StarTargets.Min() > targetValue)
                    {
                        conditionConfig.StarTargets[0] = targetValue;
                        CDebug.LogError("Minimal value for 1 star changed! (once and forever)");
                    }
                }

                isInProgress = targetValue > 0;
            }
        }

        /// <summary>
        /// Call update score of conditions if win.
        /// </summary>
        public void CallUpdateBestScore() => UpdateBestScore();

        /// <summary>
        /// Check condition met, update status.
        /// </summary>
        public abstract void UpdateConditionMet();

        /// <summary>
        /// Updaate best score/time values for level.
        /// </summary>
        protected abstract void UpdateBestScore();

        protected virtual void OnDisable()
        {
            isInProgress = false;
            isConditionMet = false;
        }

        protected void OnConditionFail()
        {
            isInProgress = false;
            isConditionMet = false;

            onConditionStatusChanged();
        }

        protected void OnConditionMet()
        {
            UpdateBestScore();

            isInProgress = false;
            isConditionMet = true;

            onConditionStatusChanged();
        }

        /// <summary>
        /// Override if need inverse initial condition status (example timer).
        /// </summary>
        protected virtual void SetDefaultConditionStatus() => isConditionMet = false;

        protected void UpdateStarsCount(int newScoreValue)
        {
            if (conditionConfig == null)
            {
                CDebug.LogError("conditionConfig is null");
                return;
            }

            if (conditionConfig.StarTargets.Count < MAX_STARS_COUNT)
            {
                CDebug.LogError("Number of stars target values not equals with maximum stars count");
            }

            int count = 0;

            foreach (int starTarget in conditionConfig.StarTargets)
            {
                if (starTarget <= newScoreValue)
                {
                    count++;
                }
            }

            StarsForCondition = Mathf.Clamp(count, 0, MAX_STARS_COUNT);
        }
    }
}