namespace DekDev.SaveSystem
{
    using DekDev.CDebug;
    using DekDev.Extensions.AbstractViews;
    using UnityEngine;

    /// <summary>
    /// Кнопка переключения между предметами вперед/назад.
    /// </summary>
    public class SelectionPrevNextButton : AbstractButtonView
    {
        [SerializeField] protected SavableSelectionInstance saveControllerContainer = default;
        [SerializeField] protected bool isNext = true;
        protected SavableSelection selection = default;
        protected SelectionItem[] items;

        protected virtual void Start() => TryInit();

        protected override void OnButtonClicked()
        {
            if (TryInit())
            {
                int currentIndex = FindItemIndex(selection.GetCurrentSelection(), items);

                int newIndex = CalculateNewIndex(currentIndex, items.Length, isNext);

                selection.Select(items[newIndex].Id);
            }
        }

        protected int FindItemIndex(SelectionItem target, SelectionItem[] items)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i].Id == target.Id)
                {
                    return i;
                }
            }

            return -1;
        }

        protected int CalculateNewIndex(int currentIndex, int itemsLength, bool moveNext)
        {
            if (moveNext)
            {
                return currentIndex >= itemsLength - 1 ? 0 : currentIndex + 1;
            }
            else
            {
                return currentIndex <= 0 ? itemsLength - 1 : currentIndex - 1;
            }
        }

        protected bool TryInit()
        {
            if (selection != null && items.Length > 0)
            {
                return true;
            }

            if (saveControllerContainer == null || !saveControllerContainer.IsInited
                || saveControllerContainer.Instance == null || saveControllerContainer.Instance.Database == null)
            {
                CDebug.LogError("Контроллер сохранений недоступен!");
                return false;
            }

            selection = saveControllerContainer.Instance;
            items = selection.Database.Items;

            if (selection == null || items.Length < 1)
            {
                CDebug.LogError("Не удалось инициализировать предметы для выбора!");
                return false;
            }

            return true;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (saveControllerContainer == null)
            {
                CDebug.LogError($"Ссылка на {nameof(saveControllerContainer)} не установлена!");
            }
        }
#endif
    }
}