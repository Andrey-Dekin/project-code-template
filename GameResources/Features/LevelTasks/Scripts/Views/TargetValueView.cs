namespace WinterDrift.Features.LevelTasks.Views
{
    using WinterDrift.Features.SaveSystem.Levels;
    using UnityEngine;

    /// <summary>
    /// Target value view.
    /// </summary>
    public class TargetValueView : AbstractLevelTextView
    {
        [SerializeField] protected LevelType targetType = LevelType.ScoreMode;

        protected override string GetValueString()
        {
            if (currentLevel != null && currentLevel != null && LevelTypeMatchCheck(targetType))
            {
                return GetTargetConditionValue(targetType).ToString();
            }

            return string.Empty;
        }
    }
}