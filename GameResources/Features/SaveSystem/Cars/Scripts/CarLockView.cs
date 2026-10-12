namespace WinterDrift.Features.SaveSystem.Cars
{
    using System.Collections.Generic;
    using DekDev.Extensions.Utils;
    using DekDev.SaveSystem;
    using UnityEngine;

    /// <summary>
    /// Switch objects by lock status.
    /// </summary>
    public class CarLockView : GenericSelectionProviderCallback<CarSelectionData>
    {
        [SerializeField] protected List<GameObject> unlockObjects = new List<GameObject>();
        [SerializeField] protected List<GameObject> lockObjects = new List<GameObject>();

        protected override void OnSelectionDataChanged(CarSelectionData selectedItem)
        {
            unlockObjects.SetStatus(selectedItem.IsUnlocked);
            lockObjects.SetStatus(!selectedItem.IsUnlocked);
        }
    }
}