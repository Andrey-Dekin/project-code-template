namespace DekDev.Extensions.Utils
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Abstract class for defining a specific component via triggers.
    /// </summary>
    public abstract class AbstractGenericTrigger<T> : MonoBehaviour where T : Component
    {
        /// <summary>
        /// Signals that a component has been received.
        /// </summary>
        public event Action onTriggerEntered = delegate { };

        /// <summary>
        /// Signals that a component has exited a trigger.
        /// </summary>
        public event Action onTriggerExit = delegate { };

        protected T targetComponent = default;

        protected virtual void OnTriggerEnter(Collider collider)
        {
            T enteredComponent = collider.GetComponent<T>() ?? collider.GetComponentInParent<T>();

            if (enteredComponent != null)
            {
                targetComponent = enteredComponent;

                OnTriggerEnterSuccessfully(targetComponent);
                onTriggerEntered();
            }
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            T exitedComponent = collider.GetComponent<T>() ?? collider.GetComponentInParent<T>();

            if (exitedComponent != null && exitedComponent == targetComponent)
            {
                OnTriggerExitSuccessfully(targetComponent);
                onTriggerExit();

                targetComponent = null;
            }
        }

        /// <summary>
        /// Override for game object successfully entered into trigger
        /// </summary>
        /// <param name="instance"></param>
        protected virtual void OnTriggerEnterSuccessfully(T instance) { }

        /// <summary>
        /// Override for game object successfully exit into trigger
        /// </summary>
        /// <param name="instance"></param>
        protected virtual void OnTriggerExitSuccessfully(T instance) { }
    }
}