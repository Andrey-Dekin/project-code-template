namespace WinterDrift.Features.SaveSystem.Levels
{
    using DekDev.CDebug;
    using DekDev.SaveSystem;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Manager for unlocking levels based on completed levels count.
    /// </summary>
    public class LevelUnlockManager : MonoBehaviour
    {
        [SerializeField] protected SelectionDatabase levelDatabase = default;
        protected int completedLevelsCount = 0;
        protected BaseLevelItem levelData = default;
        protected List<BaseLevelItem> allLevels = new List<BaseLevelItem>();

        protected virtual void Awake()
        {
            foreach (SelectionItem item in levelDatabase.Items)
            {
                if (item != null && item is BaseLevelItem levelData)
                {
                    allLevels.Add(levelData);
                }
            }
        }

        protected virtual void OnEnable()
        {
            Subscribe();

            UpdateLevelsUnlockStatus();
        }

        protected virtual void OnDisable() => Unsubscribe();

        /// <summary>
        /// Update unlock status for levels.
        /// </summary>
        public virtual void UpdateLevelsUnlockStatus()
        {
            completedLevelsCount = GetCompletedLevelsCount();

            foreach (BaseLevelItem item in allLevels)
            {
                if (item != null)
                {
                    UpdateLevelUnlockStatus(item);
                }
            }
        }

        protected void UpdateLevelUnlockStatus(BaseLevelItem levelData)
        {
            bool shouldBeUnlocked = completedLevelsCount >= levelData.TargetCompletedLevelsCount;

            if (levelData.IsUnlocked != shouldBeUnlocked)
            {
                levelData.IsUnlocked = shouldBeUnlocked;

                CDebug.Log($"Level {levelData.name}: target={levelData.TargetCompletedLevelsCount}, completed={completedLevelsCount}, unlocked={shouldBeUnlocked}");
            }
        }

        protected virtual int GetCompletedLevelsCount()
        {
            int count = 0;

            if (levelDatabase != null && levelDatabase.Items != null)
            {
                foreach (SelectionItem item in levelDatabase.Items)
                {
                    if (item != null && item is BaseLevelItem levelData && levelData.IsCompleted)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        protected virtual void Subscribe()
        {
            foreach (BaseLevelItem item in allLevels)
            {
                if (item != null)
                {
                    item.onCompleted += UpdateLevelsUnlockStatus;
                }
            }
        }

        protected virtual void Unsubscribe()
        {
            foreach (BaseLevelItem item in allLevels)
            {
                if (item != null)
                {
                    item.onCompleted -= UpdateLevelsUnlockStatus;
                }
            }
        }
    }
}