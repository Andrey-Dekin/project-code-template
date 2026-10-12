namespace DekDev.SaveSystem.Views
{
    using System.Collections.Generic;
    using DekDev.Extensions.Utils;
    using UnityEngine;

    /// <summary>
    /// Switch view objects by selected item in DB.
    /// </summary>
    public class SelectedDataObjectSwitcher : SelectionProviderCallback
    {
        [SerializeField] protected SelectionItem targetItem = default;
        [SerializeField] protected List<GameObject> enableObjectes = new List<GameObject>();
        [SerializeField] protected List<GameObject> disableObjectes = new List<GameObject>();

        protected override void OnSelectionDataChanged(SelectionItem data) => UpdateView(targetItem.Id == data.Id);

        protected void UpdateView(bool status)
        {
            enableObjectes.SetStatus(status);
            disableObjectes.SetStatus(!status);
        }
    }
}
