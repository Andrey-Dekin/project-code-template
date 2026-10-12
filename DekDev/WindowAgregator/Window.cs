namespace DekDev.WindowAgregator
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Окно.
    /// </summary>
    public class Window : MonoBehaviour
    {
        /// <summary>
        /// Событие смены фокуса.
        /// </summary>
        public event Action<bool> onWindowIsFocused = delegate { };

        /// <summary>
        /// Индификатор окна.
        /// </summary>
        public string Id => id;
        [SerializeField] protected string id = default;

        /// <summary>
        /// Окно в фокусе.
        /// </summary>
        public bool IsFocused
        {
            get => isFocused;
            set
            {
                isFocused = value;
                onWindowIsFocused(isFocused);
            }
        }

        protected bool isFocused = default;
    }
}