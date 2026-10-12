namespace DekDev.Extensions.AbstractViews
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Класс абстрактного изображения
    /// </summary>
    [RequireComponent(typeof(Image))]
    public abstract class AbstractImageView : MonoBehaviour
    {
        protected Image imageView = default;

        protected virtual void Awake() => imageView = GetComponent<Image>();

        /// <summary>
        /// Обновление изображения
        /// </summary>
        protected abstract void UpdateImageView();
    }
}
