namespace DekDev.Extensions.AbstractViews
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Класс абстрактного слайдера
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public abstract class AbstractSliderView : MonoBehaviour
    {
        protected Slider sliderView = default;

        protected virtual void Awake()
        {
            sliderView = GetComponent<Slider>();
            sliderView.onValueChanged.AddListener(OnValueChanged);
        }

        protected virtual void OnDestroy() => sliderView.onValueChanged.RemoveListener(OnValueChanged);

        /// <summary>
        /// Изменение действия по изменению значения
        /// </summary>
        protected abstract void OnValueChanged(float newValue);
    }
}