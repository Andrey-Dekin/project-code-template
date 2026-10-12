namespace DekDev.Extensions.Utils
{
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.InputSystem;

    /// <summary>
    /// Event caller on target key down.
    /// </summary>
    public class ButtonKeyEventCaller : MonoBehaviour
    {
        /// <summary>
        /// Target events.
        /// </summary>
        public UnityEvent customEventOnKey = default;

        [SerializeField] protected Key key = default;
        protected InputAction inputAction = default;

        protected virtual void Awake()
        {
            inputAction = new InputAction($"CustomKeyAction_{gameObject.name}_{key}");
            inputAction.AddBinding($"<Keyboard>/{key}");
        }

        protected virtual void OnEnable()
        {
            inputAction.performed += OnPerformed;
            inputAction.Enable();
        }

        protected virtual void OnDisable()
        {
            inputAction.performed -= OnPerformed;
            inputAction.Disable();
        }

        protected virtual void OnDestroy() => inputAction?.Dispose();

        protected virtual void OnPerformed(InputAction.CallbackContext context) => Action();

        protected virtual void Action() => customEventOnKey?.Invoke();
    }
}