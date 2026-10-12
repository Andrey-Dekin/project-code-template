namespace DekDev.Extensions.Facade
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Абстрактный контейнер
    /// </summary>
    public abstract class AbstractFacadeContainer<T> : ScriptableObject, IDisposable where T : AbstractFacade
    {
        /// <summary>
        /// Сигнализирует о том, что экземпляр инициирован
        /// </summary>
        public event Action onInstanceInited = delegate { };

        /// <summary>
        /// Сигнализирует о том, что экземпляр отчищен
        /// </summary>
        public event Action onInstanceDisposed = delegate { };

        /// <summary>
        /// Фасад
        /// </summary>
        public T Facade
        {
            get => facade;
            
            protected set
            {
                if (value != null)
                {
                    facade = value;
                    onInstanceInited();
                    return;
                }

                if (facade != null)
                {
                    facade = null;
                    onInstanceDisposed();
                }
            }
        }
        [SerializeField] protected string id = string.Empty;
        [SerializeField] protected T facade = default;

        /// <summary>
        /// Инициализация экземпляра фасада
        /// </summary>
        /// <param name="facade"></param>
        public virtual void Init(T facade) => Facade = facade;

        /// <summary>
        /// Отчистка
        /// </summary>
        public virtual void Dispose() => Facade = null;
    }
}