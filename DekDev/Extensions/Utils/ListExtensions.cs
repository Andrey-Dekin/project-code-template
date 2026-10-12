namespace DekDev.Extensions.Utils
{
    using System.Collections;
    using System.Collections.Generic;
    using Unity.VisualScripting;
    using UnityEngine;

    /// <summary>
    /// Extensions for lists.
    /// </summary>
    public static class ListExtensions
    {
        public static void SetStatus(this List<GameObject> list, bool isActive)
        {
            if (list != null && list.Count > 0)
            {
                foreach (GameObject item in list)
                {
                    if (item != null)
                    {
                        item.SetActive(isActive);
                    }
                }
            }
        }
    }
}
