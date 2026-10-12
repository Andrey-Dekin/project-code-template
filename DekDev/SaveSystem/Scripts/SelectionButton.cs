namespace DekDev.SaveSystem
{
    using DekDev.CDebug;
    using DekDev.Extensions.AbstractViews;
    using UnityEngine;

    /// <summary>
    /// Кнопка выбора элемента с сохранением состояния.
    /// </summary>
    public class SelectionButton : AbstractButtonView
    {
        [SerializeField] protected SavableSelectionInstance saveControllerContainer = default;
        [SerializeField] protected SelectionItem targetData = default;

        protected override void OnButtonClicked()
        {
            if (saveControllerContainer == null || !saveControllerContainer.IsInited)
            {
                CDebug.LogError("Не удалось выбрать предмет, контроллер недоступен!");
                return;
            }

            if (targetData == null)
            {
                return;
            }
            
            saveControllerContainer.Instance.Select(targetData.Id);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (targetData == null)
            {
                CDebug.LogError($"Сериализованный объект {nameof(targetData)} не заполнен");
            }
        }
#endif
    }
}