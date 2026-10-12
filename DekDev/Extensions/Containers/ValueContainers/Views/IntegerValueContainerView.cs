namespace DekDev.Extensions.Containers.Views
{
    using DekDev.Extensions.AbstractViews;
    using DekDev.Extensions.Containers.ValueContainers;
    using UnityEngine;

    /// <summary>
    /// Text view for integer value container.
    /// </summary>
    public class IntegerValueContainerView : AbstractTextView
    {
        [SerializeField] protected IntegerValueContainer valueContainer = default;

        protected virtual void OnEnable()
        {
            valueContainer.onValueChanged += UpdateTextView;

            UpdateTextView();
        }

        protected virtual void OnDisable() => valueContainer.onValueChanged -= UpdateTextView;

        protected override void UpdateTextView() => textView.text = valueContainer.Value.ToString();
    }
}
