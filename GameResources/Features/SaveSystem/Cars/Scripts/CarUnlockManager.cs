namespace WinterDrift.Features.SaveSystem.Cars
{
    using DekDev.CDebug;
    using DekDev.SaveSystem;
    using System.Collections.Generic;
    using UnityEngine;
    using WinterDrift.Features.SaveSystem.Cars;
    using WinterDrift.Features.SaveSystem.Levels;

    /// <summary>
    /// Manager for unlocking levels based on completed levels count.
    /// </summary>
    public class CarUnlockManager : LevelUnlockManager
    {
        [SerializeField] protected SelectionDatabase carDatabase = default;

        protected CarSelectionData carData = default;
        protected List<CarSelectionData> allCars = new List<CarSelectionData>();

        protected override void Awake()
        {
            foreach (SelectionItem item in carDatabase.Items)
            {
                if (item != null && item is CarSelectionData carData)
                {
                    allCars.Add(carData);
                }
            }
        }

        /// <summary>
        /// Update unlock status for cars.
        /// </summary>
        public override void UpdateLevelsUnlockStatus()
        {
            completedLevelsCount = GetCompletedLevelsCount();

            foreach (CarSelectionData item in allCars)
            {
                if (item != null)
                {
                    UpdateLevelUnlockStatus(item);
                }
            }
        }

        protected void UpdateLevelUnlockStatus(CarSelectionData carData)
        {
            bool shouldBeUnlocked = completedLevelsCount >= carData.TargetCompletedLevelsCount;

            if (carData.IsUnlocked != shouldBeUnlocked)
            {
                carData.IsUnlocked = shouldBeUnlocked;

                CDebug.Log($"Car {carData.name}: target={carData.TargetCompletedLevelsCount}, completed={completedLevelsCount}, unlocked={shouldBeUnlocked}");
            }
        }

        protected override int GetCompletedLevelsCount()
        {
            int count = 0;

            if (levelDatabase != null && levelDatabase.Items != null)
            {
                foreach (BaseLevelItem item in levelDatabase.Items)
                {
                    if (item != null && item is BaseLevelItem levelData && levelData.IsCompleted)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        protected override void Subscribe()
        {
            //NOTE: Переопределен чтобы не было подписки
        }

        protected override void Unsubscribe()
        {
            //NOTE: Переопределен чтобы не было подписки
        }
    }
}