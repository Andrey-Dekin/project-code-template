namespace DekDev.Extensions.AbstractViews
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Класс абстрактного выпадающего списка
    /// </summary>
    [RequireComponent(typeof(Dropdown))]
    public abstract class AbstractDropdownView : MonoBehaviour
    {
        protected Dropdown dropdownView = default;

        protected virtual void Awake()
        {
            dropdownView = GetComponent<Dropdown>();
            dropdownView.onValueChanged.AddListener(OnValueChanged);
        }

        protected virtual void OnDestroy() => dropdownView.onValueChanged.RemoveListener(OnValueChanged);

        /// <summary>
        /// Изменения при выборе элемента в выпадающем списке
        /// </summary>
        protected abstract void OnValueChanged(int index);
    }
}