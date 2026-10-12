namespace DekDev.Extensions.AbstractViews
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Класс абстрактного поля ввода
    /// </summary>
    [RequireComponent(typeof(InputField))]
    public abstract class AbstractInputFieldView : MonoBehaviour
    {
        protected InputField inputFieldView = default;

        protected virtual void Awake()
        {
            inputFieldView = GetComponent<InputField>();
            inputFieldView.onValueChanged.AddListener(OnValueChanged);
        }

        protected virtual void OnDestroy() => inputFieldView.onValueChanged.RemoveListener(OnValueChanged);

        /// <summary>
        /// Изменения при обновлении строки в поле ввода
        /// </summary>
        protected abstract void OnValueChanged(string input);
    }
}