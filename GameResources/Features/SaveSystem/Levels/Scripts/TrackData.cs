namespace WinterDrift.Features.SaveSystem.Levels
{
    using DekDev.Extensions.Id;
    using UnityEngine;

    /// <summary>
    /// Дата гоночного трека.
    /// </summary>
    [CreateAssetMenu(menuName = "WinterDrift/Features/Levels/" + nameof(TrackData))]
    public class TrackData : ScriptableObject
    {
        /// <summary>
        /// Идентификатор.
        /// </summary>
        public string Id => id.Id;

        /// <summary>
        /// Ключ локализации названия уровня.
        /// </summary>
        public string NameKey => nameKey;

        /// <summary>
        /// Спрайт карты трека.
        /// </summary>
        public Sprite Map => map;

        /// <summary>
        /// Префаб для спауна границ трека.
        /// </summary>
        public GameObject Prefab => prefab;

        [SerializeField] protected ID id = default;
        [SerializeField] protected string nameKey = string.Empty;
        [SerializeField] protected Sprite map = default;
        [SerializeField] protected GameObject prefab = default;
    }
}
