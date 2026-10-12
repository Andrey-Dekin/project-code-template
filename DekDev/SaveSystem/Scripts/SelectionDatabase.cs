namespace DekDev.SaveSystem
{
    using UnityEngine;

    /// <summary>
    /// Класс базы данных сохраняемых предметов.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SelectionDatabase), menuName = "DekDev/SaveSystem/" + nameof(SelectionDatabase))]

    public class SelectionDatabase : ScriptableObject
    {
        /// <summary>
        /// Элемент по умолчанию.
        /// </summary>
        public SelectionItem DefaultItem => defaultItem;

        /// <summary>
        /// Список элементов базы данных.
        /// </summary>
        public SelectionItem[] Items => items;

        [Header("Дата которая будет выбрана по умолчанию")]
        [SerializeField] protected SelectionItem defaultItem = default;

        [Header("Список всех предметов для выбора")]
        [SerializeField] protected SelectionItem[] items;

        /// <summary>
        /// Получить элемент по id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SelectionItem GetItem(string id)
        {
            foreach (SelectionItem item in items)
            {
                if (item.Id.Equals(id))
                {
                    return item;
                }
            }

            return defaultItem;
        }
    }
}