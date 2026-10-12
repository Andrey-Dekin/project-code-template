namespace DekDev.Extensions.Containers.Views
{
    using DekDev.Extensions.AbstractViews;
    using DekDev.Extensions.Containers.ValueContainers;
    using UnityEngine;

    /// <summary>
    /// Text view for string value container.
    /// </summary>
    public class StringValueContainerView : AbstractTextView
    {
        [SerializeField] protected StringValueContainer valueContainer = default;

        protected virtual void OnEnable() => UpdateTextView();

        protected override void UpdateTextView() => textView.text = valueContainer.Value;
    }
}
