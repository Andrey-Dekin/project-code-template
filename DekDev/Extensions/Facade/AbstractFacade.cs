namespace DekDev.Extensions.Facade
{
    using UnityEngine;

    /// <summary>
    /// Абстрактный фасад
    /// </summary>
    public abstract class AbstractFacade : MonoBehaviour
    {
        /// <summary>
        /// Идентификатор фасада
        /// </summary>
        public string Id => id;
        [SerializeField] protected string id = string.Empty;
    }
}