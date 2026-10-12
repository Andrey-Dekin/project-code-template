namespace DekDev.Extensions.Facade
{
    using UnityEngine;

    /// <summary>
    /// Абстрактный инсталлер
    /// </summary>
    public abstract class AbstractInstaller<T, B> : MonoBehaviour
                                        where T : AbstractFacade
                                        where B : AbstractFacadeContainer<T>
    {
        [SerializeField] protected T facade = default;
        [SerializeField] protected B container = default;

        protected virtual void Start()
        {
            if (facade != null && container != null)
            {
                container.Init(facade);
            }
        }

        protected virtual void OnDestroy()
        {
            if (container != null)
            {
                container.Dispose();
            }
        }
    }
}