namespace DekDev.Extensions.Containers
{
    using UnityEngine;

    /// <summary>
    /// Абстрактный инициализатор компонента в контейнер.
    /// </summary>
    public class AbstractComponentInitializer<T> : MonoBehaviour where T : Component
    {
        [SerializeField] protected T targetComponent = default;
        [SerializeField] protected AbstractComponentContainer<T> container = default;

        protected virtual void Awake() => container.Component = targetComponent;

        protected virtual void OnDestroy() => container.Component = null;
    }
}