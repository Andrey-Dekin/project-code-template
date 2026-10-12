namespace DekDev.Extensions.Id
{
    using UnityEngine;

    /// <summary>
    /// Класс строкового индификатора.
    /// </summary>
    [CreateAssetMenu(menuName = "DekDev/Extensions/" + nameof(ID))]
    public class ID : ScriptableObject
    {
        /// <summary>
        /// Строковый идентификатор.
        /// </summary>
        public string Id => id;

        [SerializeField] protected string id = string.Empty;
    }
}