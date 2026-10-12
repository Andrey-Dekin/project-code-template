namespace DekDev.SaveSystem.Views
{
    using System;
    using System.Collections.Generic;
    using DekDev.Extensions.Utils;
    using UnityEngine;

    /// <summary>
    /// Switch object with check current selected data.
    /// </summary>
    public class FixedListObjectSwitcher : SelectionProviderCallback
    {
        [SerializeField] protected List<SelectedDataObjects> switchItems = new List<SelectedDataObjects>();
        protected string currentId = string.Empty;
        protected SelectedDataObjects currentObjectData = default;

        protected override void OnSelectionDataChanged(SelectionItem data)
        {
            if (data != null && currentId != data.Id)
            {
                currentObjectData = switchItems.Find(x => x.TargetData.Id == data.Id);

                if (currentObjectData != null)
                {
                    switchItems.ForEach(x => x.GameObjects.SetStatus(false));

                    currentObjectData.GameObjects.SetStatus(true);

                    currentId = data.Id;
                }
            }
        }
    }
}