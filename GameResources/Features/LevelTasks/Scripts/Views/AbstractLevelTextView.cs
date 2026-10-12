namespace WinterDrift.Features.LevelTasks.Views
{
    using DekDev.CDebug;
    using UnityEngine;
    using UnityEngine.UI;
    using WinterDrift.Features.SaveSystem.Levels;

    /// <summary>
    /// Abstract text view for level values.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public abstract class AbstractLevelTextView : AbstractLevelView
    {
        protected Text textView = default;
        protected BaseLevelItem currentLevel = default;
        private string _targetValue = string.Empty;

        protected virtual void Awake() => textView = GetComponent<Text>();

        protected override void UpdateView()
        {
            _targetValue = GetValueString();

            if (!string.IsNullOrEmpty(_targetValue))
            {
                textView.text = _targetValue;
            }
            else
            {
                textView.text = "";
            }
        }

        protected override void OnLevelInited()
        {
            base.OnLevelInited();

            currentLevel = levelController.CurrentLevel;

            if (currentLevel == null)
            {
                CDebug.LogError("Could't get current level in LevelController");
            }
        }

        protected virtual bool LevelTypeMatchCheck(LevelType targetLevelType)
        {
            if (currentLevel != null)
            {
                return currentLevel.Conditions.Find(x => x.ConditionType == targetLevelType) != null;
            }

            return false;
        }

        protected virtual int GetTargetConditionValue(LevelType targetLevelType)
        {
            LevelConditionConfig _levelConfig = default;

            if (currentLevel != null)
            {
                _levelConfig = currentLevel.Conditions.Find(x => x.ConditionType == targetLevelType);

                if (_levelConfig != null)
                {
                    return _levelConfig.TargetValue;
                }
            }

            return 0;
        }

        /// <summary>
        /// Take string value for text view.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetValueString();
    }
}