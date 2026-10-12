namespace WinterDrift.Features.SaveSystem.Levels
{
    using System.Linq;
    using DekDev.SaveSystem;
    using UnityEngine;
    using WinterDrift.Features.SaveSystem.Cars;

    /// <summary>
    /// Reset locked cars on Disable.
    /// </summary>
    public class ResetLockedCarOnDisable : GenericSelectionProviderCallback<CarSelectionData>
    {
        [SerializeField] protected SelectionDatabase database = default;
        [SerializeField] protected SavableSelectionInstance saveControllerContainer = default;
        protected CarSelectionData currentCar = default;

        protected override void OnDisable()
        {
            base.OnDisable();

            ResetLockedData();
        }

        protected override void OnSelectionDataChanged(CarSelectionData selectedItem)
        => currentCar = selectedItem;

        /// <summary>
        /// Reset on last unlocked or first.
        /// </summary>
        protected virtual void ResetLockedData()
        {
            if (!currentCar.IsUnlocked)
            {
                SelectionItem unlockedCar = database.Items.LastOrDefault(x => x is CarSelectionData carItem && carItem.IsUnlocked);

                saveControllerContainer.Instance.Select(unlockedCar != null ? unlockedCar.Id : database.Items[0].Id);
            }
        }
    }
}