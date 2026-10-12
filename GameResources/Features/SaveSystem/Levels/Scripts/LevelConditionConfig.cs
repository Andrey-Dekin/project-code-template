namespace WinterDrift.Features.SaveSystem.Levels
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Serializable class for set level conditions in inspector.
    /// </summary>
    [Serializable]
    public class LevelConditionConfig
    {
        /// <summary>
        /// Type of condition mode.
        /// </summary>
        public LevelType ConditionType = LevelType.ScoreMode;

        /// <summary>
        /// Target value(score/seconds).
        /// </summary>
        public int TargetValue = 0;

        /// <summary>
        /// Target values for rating level complete (stars).
        /// </summary>
        public List<int> StarTargets = new List<int>();
    }
}