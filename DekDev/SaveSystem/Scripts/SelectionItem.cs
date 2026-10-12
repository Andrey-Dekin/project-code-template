namespace DekDev.SaveSystem
{
    using DekDev.Extensions.Id;
    using UnityEngine;

    /// <summary>
    /// Абстрактный класс элемента БД для выбора.
    /// </summary>
    /// <typeparam name="string"></typeparam>
    [CreateAssetMenu(menuName = "DekDev/SaveSystem/" + nameof(SelectionItem))]
    public class SelectionItem : ScriptableObject
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id => id.Id;

        [SerializeField] protected ID id = default;
    }
}