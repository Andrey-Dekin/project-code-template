namespace DekDev.WindowAgregator
{
    using DekDev.Extensions.Utils;
    using UnityEngine;

    /// <summary>
    /// Close window on Escape.
    /// </summary>
    public class ButtonEscape : ButtonKeyEventCaller
    {
        [SerializeField] protected Window window = default;
        [SerializeField] protected bool closeOnKey = default;

        protected override void Action()
        {
            if (window.IsFocused)
            {
                customEventOnKey?.Invoke();

                if (closeOnKey)
                {
                    WindowAgregator.onWindowClose();
                }
            }
        }
    }
}