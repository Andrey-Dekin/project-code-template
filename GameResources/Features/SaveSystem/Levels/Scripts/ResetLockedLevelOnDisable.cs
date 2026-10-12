namespace WinterDrift.Features.SaveSystem.Levels
{
    using System.Linq;
    using DekDev.SaveSystem;
    using UnityEngine;

    /// <summary>
    /// Reset locked levels on Disable.
    /// </summary>
    public class ResetLockedLevelOnDisable : GenericSelectionProviderCallback<BaseLevelItem>
    {
        [SerializeField] protected SelectionDatabase database = default;
        [SerializeField] protected SavableSelectionInstance saveControllerContainer = default;
        protected BaseLevelItem currentLevel = default;

        protected override void OnDisable()
        {
            base.OnDisable();

            ResetLockedData();
        }

        protected override void OnSelectionDataChanged(BaseLevelItem selectedItem)
        => currentLevel = selectedItem;

        /// <summary>
        /// Reset on last unlocked and not completed or first.
        /// </summary>
        protected virtual void ResetLockedData()
        {
            if (!currentLevel.IsUnlocked)
            {
                SelectionItem unlockedLevel = database.Items.LastOrDefault(x => x is BaseLevelItem levelItem && levelItem.IsUnlocked && !levelItem.IsCompleted);

                saveControllerContainer.Instance.Select(unlockedLevel != null ? unlockedLevel.Id : database.Items[0].Id);
            }
        }
    }
}