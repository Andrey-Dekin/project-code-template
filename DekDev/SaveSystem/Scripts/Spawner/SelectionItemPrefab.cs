namespace DekDev.SaveSystem.Spawner
{
    using DekDev.Extensions.Id;
    using UnityEngine;

    /// <summary>
    /// Префаб связанный с датой по Id.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(SelectionItemPrefab), menuName = "DekDev/SaveSystem/Spawner/" + nameof(SelectionItemPrefab))]
    public class SelectionItemPrefab : ScriptableObject
    {
        /// <summary>
        /// Идентификатор связанный с выбранной датой.
        /// </summary>
        public string Id => id.Id;

        /// <summary>
        /// Префаб.
        /// </summary>
        public GameObject Prefab => prefab;

        [SerializeField] protected ID id = default;
        [SerializeField] protected GameObject prefab = default;
    }
}
