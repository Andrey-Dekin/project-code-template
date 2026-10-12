namespace DekDev.SaveSystem
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Контейнер с экземпляром контроллера сохранений.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SavableSelectionInstance), menuName = "DekDev/SaveSystem/" + nameof(SavableSelectionInstance))]
    public class SavableSelectionInstance : ScriptableObject
    {
        /// <summary>
        /// Событие инициализации экземпляра.
        /// </summary>
        public event Action onInstanceInited = delegate { };

        /// <summary>
        /// Экземпляр контроллера сохранений.
        /// </summary>
        public SavableSelection Instance => instance;

        /// <summary>
        /// Экземпляр инициализирован.
        /// </summary>
        public bool IsInited => isInited;

        protected SavableSelection instance = default;
        protected bool isInited = false;

        /// <summary>
        /// Инициализация экземпляра контроллера.
        /// </summary>
        /// <param name="savableSelectioninstance"></param>
        public void Initialize(SavableSelection savableSelectioninstance)
        {
            instance = savableSelectioninstance;

            isInited = instance != null;

            if (isInited)
            {
                onInstanceInited();
            }
        }
    }
}
