namespace DekDev.Extensions.AbstractViews
{
    using UnityEngine.UI;
    using UnityEngine;

    /// <summary>
    /// Класс абстрактной кнопки
    /// </summary>
    [RequireComponent(typeof(Button))]
    public abstract class AbstractButtonView : MonoBehaviour
    {
        protected Button buttonView = default;

        protected virtual void Awake()
        {
            buttonView = GetComponent<Button>();
            buttonView.onClick.AddListener(OnButtonClicked);
        }

        protected virtual void OnDestroy() => buttonView.onClick.RemoveListener(OnButtonClicked);

        /// <summary>
        /// Действия по нажатию на кнопку
        /// </summary>
        protected abstract void OnButtonClicked();
    }
}