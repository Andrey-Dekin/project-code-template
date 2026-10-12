namespace WinterDrift.Features.LevelTasks.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DekDev.CDebug;
    using DekDev.Extensions.Utils;
    using UnityEngine;
    using WinterDrift.Features.SaveSystem.Levels;

    /// <summary>
    /// Switch objects by active mode conditions.
    /// </summary>
    public class ModeTypeSwitcher : AbstractLevelView
    {
        [SerializeField] protected List<ModeObjects> modeObjects = new List<ModeObjects>();
        protected BaseLevelItem currentLevel = default;

        protected override void UpdateView()
        {
            if (currentLevel == null)
            {
                InitCurrentLevel();
            }

            if (currentLevel == null)
            {
                CDebug.LogError("Couldn't get current level for view");
                return;
            }

            modeObjects.ForEach(x => x.Objects.SetStatus(false));

            foreach (LevelConditionConfig condition in currentLevel.Conditions)
            {
                modeObjects.ForEach(x =>
                {
                    if (x.Types.Contains(condition.ConditionType))
                    {
                        x.Objects.SetStatus(true);
                    }
                });
            }
        }

        protected void InitCurrentLevel()
        {
            currentLevel = levelController.CurrentLevel;
        }
    }
}