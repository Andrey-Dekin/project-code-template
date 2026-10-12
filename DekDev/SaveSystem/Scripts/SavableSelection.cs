namespace DekDev.SaveSystem
{
    using UnityEngine;
    using System;
    using DekDev.Extensions.Id;
    using DekDev.CDebug;

#if RedefinePlayerPrefs_yg && UNITY_WEBGL
    using PlayerPrefs = RedefineYG.PlayerPrefs;
#endif

    /// <summary>
    /// Основной контроллер сохранения выбранных предметов в префсы.
    /// </summary>
    public class SavableSelection : MonoBehaviour
    {
        /// <summary>
        /// Событие смены выбранной даты.
        /// </summary>
        public event Action onSelectionChanged = delegate { };

        /// <summary>
        /// БД предметов для выбора.
        /// </summary>
        public SelectionDatabase Database => database;

        [SerializeField] protected ID saveKey = default;
        [SerializeField] protected SelectionDatabase database = default;
        [SerializeField] protected SavableSelectionInstance instanceContainer = default;

        protected string currentId = string.Empty;
        protected SelectionItem currentData = default;

        /// <summary>
        /// Выбрать дату по id.
        /// </summary>
        /// <param name="id"></param>
        public virtual void Select(string id)
        {
            if (currentId != string.Empty && currentId.Equals(id))
            {
                return;
            }

            currentId = id;
            currentData = database.GetItem(id);
            SaveSelection();
            onSelectionChanged?.Invoke();
        }

        /// <summary>
        /// Получить текущую выбранную дату.
        /// </summary>
        /// <returns></returns>
        public SelectionItem GetCurrentSelection() => currentData;

        protected virtual void Awake()
        {
            LoadSelection();

            instanceContainer.Initialize(this);
        }

        protected virtual void SaveSelection()
        {
            PlayerPrefs.SetString(saveKey.Id, currentId);
            PlayerPrefs.Save();
        }

        protected virtual void LoadSelection()
        {
            string savedValue = PlayerPrefs.GetString(saveKey.Id, string.Empty);

            Select(string.IsNullOrEmpty(savedValue)
                ? database.DefaultItem.Id
                : savedValue);
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (instanceContainer == null || database == null || saveKey == null)
            {
                CDebug.LogError("Необходимо заполнить все сериализованные поля для корректной работы системы сохранений");
            }
        }
#endif
    }
}