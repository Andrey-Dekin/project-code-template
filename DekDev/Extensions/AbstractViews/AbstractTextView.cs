namespace DekDev.Extensions.AbstractViews
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Класс абстрактного текста
    /// </summary>
    [RequireComponent(typeof(Text))]
    public abstract class AbstractTextView : MonoBehaviour
    {
        protected Text textView = default;

        protected virtual void Awake() => textView = GetComponent<Text>();

        /// <summary>
        /// Обновление содержимого текста
        /// </summary>
        protected abstract void UpdateTextView();
    }
}
