namespace DekDev.Extensions.Facade
{
    using UnityEngine;

    /// <summary>
    /// Абстрактный провайдер
    /// </summary>
    public abstract class AbstractProvider<T,B> : MonoBehaviour
                                            where T : AbstractFacadeContainer<B>
                                            where B : AbstractFacade
    {
        [SerializeField] protected T container = default;
        protected B facade = default;

        protected virtual void OnEnable()
        {
            if(container != null)
            {
                if(container.Facade != null)
                {
                    OnInstanceInited();
                }

                container.onInstanceInited += OnInstanceInited;
                container.onInstanceDisposed += OnInstanceDisposed;
            }
        }

        protected virtual void OnDisable()
        {
            if(container != null)
            {
                container.onInstanceInited -= OnInstanceInited;
                container.onInstanceDisposed -= OnInstanceDisposed;
            }

            facade = null;
        }

        protected virtual void OnInstanceInited() => facade = container != null ? container.Facade : null;
        protected virtual void OnInstanceDisposed() => facade = null;
    }
}