namespace DekDev.Extensions.AbstractViews
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Класс абстрактного тоггла
    /// </summary>
    [RequireComponent(typeof(Toggle))]
    public abstract class AbstractToggleView : MonoBehaviour
    {
        protected Toggle toggleView = default;

        protected virtual void Awake()
        {
            toggleView = GetComponent<Toggle>();
            toggleView.onValueChanged.AddListener(OnValueChanged);
        }

        protected virtual void OnDestroy() => toggleView.onValueChanged.RemoveListener(OnValueChanged);

        /// <summary>
        /// Изменение действия по изменению значения тоггла
        /// </summary>
        protected abstract void OnValueChanged(bool isOn);
    }
}