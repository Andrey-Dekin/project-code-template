namespace WinterDrift.Features.LevelTasks.Views
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using WinterDrift.Features.SaveSystem.Levels;
    
    /// <summary>
    /// Class for serialize objects with mode types.
    /// </summary>
    [Serializable]
    public class ModeObjects
    {
        /// <summary>
        /// Target types.
        /// </summary>
        public List<LevelType> Types = new List<LevelType>();

        /// <summary>
        /// Onjects to enable.
        /// </summary>
        public List<GameObject> Objects = new List<GameObject>();
    }
}