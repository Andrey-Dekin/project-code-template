namespace DekDev.SaveSystem
{
    using UnityEngine;
    using DekDev.CDebug;

    /// <summary>
    /// Дженерик класс отслеживающий изменения выбранной даты.
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class GenericSelectionProviderCallback<T> : SelectionProviderCallback where T : SelectionItem
    {
        protected T currentItem = default;

        protected override void OnSelectionDataChanged(SelectionItem selectedItem)
        {
            currentItem = selectedItem as T;

            if (currentItem == null)
            {
                CDebug.LogError("Не удалось получить выбранный предмет");
            }
            else
            {
                OnSelectionDataChanged(currentItem);
            }
        }

        /// <summary>
        /// Действия по событию изменения выбранной даты с передачей выбранной даты.
        /// </summary>
        abstract protected void OnSelectionDataChanged(T selectedItem);

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (typeof(T) == typeof(SelectionItem))
            {
                CDebug.LogError($"Используй {nameof(SelectionProviderCallback)} вместо этого класса");
            }
        }
#endif
    }
}