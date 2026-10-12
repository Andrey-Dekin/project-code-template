namespace DekDev.Extensions.Facade
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Абстрактный контроллер фасада
    /// </summary>
    public abstract class AbstractControllerFacade<T> : MonoBehaviour, IDisposable where T : AbstractFacade
    {
        public string Id => id;

        [SerializeField] protected string id = string.Empty;

        protected T facadeInstance = default;

        private bool disposed = false;

        /// <summary>
        /// Инициализация фасада
        /// </summary>
        /// <param name="facade"></param>
        public virtual void Initialize(T facade)
        {
            if (facade == null) return;
            
            facadeInstance = facade;
            InitModule();
        }

        /// <summary>
        /// Инициализация модуля
        /// </summary>
        public virtual void InitModule(){}

        /// <summary>
        /// Отчистка модуля
        /// </summary>
        public virtual void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                facadeInstance = null;
            }
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }
    }
}