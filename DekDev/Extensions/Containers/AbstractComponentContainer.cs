namespace DekDev.Extensions.Containers
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Абстрактный класс для контейнера компонента.
    /// </summary>
    public class AbstractComponentContainer<T> : ScriptableObject where T : Component
    {
        /// <summary>
        /// Изменен компонент.
        /// </summary>
        public event Action onComponentChanged = delegate { };

        /// <summary>
        /// Компонент инициализирован.
        /// </summary>
        public bool IsInited => Component != null;

        /// <summary>
        /// Компонент.
        /// </summary>
        public T Component
        {
            get => component;
            set
            {
                component = value;

                if (component != null)
                {
                    onComponentChanged();
                }
            }
        }

        [SerializeField, Header("Устанавливается автоматически инициализатором на Awake")]
        protected T component = default;
    }
}
