namespace DekDev.SaveSystem.Views
{
    using System;
    using System.Collections.Generic;
    using DekDev.SaveSystem;
    using UnityEngine;

    /// <summary>
    /// Class for serialization.
    /// </summary>
    [Serializable]
    public class SelectedDataObjects
    {
        public SelectionItem TargetData = default;
        public List<GameObject> GameObjects = new List<GameObject>();
    }
}