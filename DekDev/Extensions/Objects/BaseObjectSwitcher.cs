namespace DekDev.Extensions.Objects
{
    using System.Collections;
    using System.Collections.Generic;
    using DekDev.Extensions.Utils;
    using UnityEngine;

    /// <summary>
    /// Base object switcher class.
    /// </summary>
    public class BaseObjectSwitcher : MonoBehaviour
    {
        [SerializeField] protected List<GameObject> enableObjects = new List<GameObject>();
        [SerializeField] protected List<GameObject> disableObjects = new List<GameObject>();

        protected virtual void SwitchObjects()
        {
            enableObjects.SetStatus(true);
            disableObjects.SetStatus(false);
        }

        protected virtual void SwitchObjects(bool status)
        {
            enableObjects.SetStatus(status);
            disableObjects.SetStatus(!status);
        }
    }
}