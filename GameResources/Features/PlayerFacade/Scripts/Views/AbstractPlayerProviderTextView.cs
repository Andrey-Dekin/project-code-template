namespace WinterDrift.Features.PlayerFacade.Views
{
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Abstract class for text views using player facade.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public abstract class AbstractPlayerProviderTextView : PlayerFacadeProvider
    {
        protected Text textView = default;

        protected virtual void Awake() => textView = GetComponent<Text>();

        protected override void OnInstanceDisposed()
        {
            base.OnInstanceDisposed();

            Unsubscribe();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Unsubscribe();
        }

        /// <summary>
        /// Update text view.
        /// </summary>
        protected abstract void UpdateTextView();

        /// <summary>
        /// Unsubscribing from events.
        /// </summary>
        protected abstract void Unsubscribe();
    }
}