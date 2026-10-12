namespace DekDev.Extensions.Facade
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Абстрактная модель фасада
    /// </summary>
    public abstract class AbstractModelFacade<T> : ScriptableObject, IDisposable where T : AbstractFacade
    {
        public string Id => id;

        [SerializeField] protected string id = string.Empty;

        protected T facadeInstance = default;

        private bool disposed = false;

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="playerFacade"></param>
        public virtual void Initialize(T facade)
        {
            if (facade == null) return;
            
            facadeInstance = facade;
            InitModel();
        }

        /// <summary>
        /// Инициализация модели
        /// </summary>
        public virtual void InitModel(){}

        /// <summary>
        /// Отчистка
        /// </summary>
        public virtual void Dispose()
        {
            if (!disposed)
            {
                disposed = true;
                facadeInstance = null;
            }
        }

        protected virtual void OnDisable()
        {
            Dispose();
        }
    }
}