namespace WinterDrift.Features.SaveSystem.Levels
{
    using System.Collections.Generic;
    using DekDev.Extensions.Utils;
    using DekDev.SaveSystem;
    using UnityEngine;

    /// <summary>
    /// Switch objects by level lock status.
    /// </summary>
    public class LevelLockView : GenericSelectionProviderCallback<BaseLevelItem>
    {
        [SerializeField] protected List<GameObject> unlockObjects = new List<GameObject>();
        [SerializeField] protected List<GameObject> lockObjects = new List<GameObject>();

        protected override void OnSelectionDataChanged(BaseLevelItem selectedItem)
        {
            unlockObjects.SetStatus(selectedItem.IsUnlocked);
            lockObjects.SetStatus(!selectedItem.IsUnlocked);
        }
    }
}