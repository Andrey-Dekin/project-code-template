namespace DekDev.SaveSystem
{
    using UnityEngine;
    using DekDev.CDebug;

    /// <summary>
    /// Класс отслеживающий изменения выбранной даты.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class SelectionProviderCallback : MonoBehaviour
    {
        [SerializeField] protected SavableSelectionInstance instanceContainer = default;
        protected bool isInstanceAvailable => instanceContainer != null && instanceContainer.IsInited;

        protected virtual void OnEnable()
        {
            if (isInstanceAvailable)
            {
                instanceContainer.Instance.onSelectionChanged += HandleSelectionChanged;

                HandleSelectionChanged();
            }
        }

        protected virtual void OnDisable()
        {
            if (isInstanceAvailable)
            {
                instanceContainer.Instance.onSelectionChanged -= HandleSelectionChanged;
            }
        }

        protected virtual void HandleSelectionChanged() => OnSelectionDataChanged(instanceContainer.Instance.GetCurrentSelection());

        /// <summary>
        /// Действия по событию изменения выбранной даты.
        /// </summary>
        /// <param name="data"></param>
        protected virtual void OnSelectionDataChanged(SelectionItem data)
        {
            //TODO: Переопределить и добавить реализацию в наследниках.
        }
    }
}