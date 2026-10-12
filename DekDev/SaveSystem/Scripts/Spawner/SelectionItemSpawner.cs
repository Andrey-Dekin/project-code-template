namespace DekDev.SaveSystem.Spawner
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using DekDev.CDebug;

    /// <summary>
    /// Спаунер выбранной даты.
    /// </summary>
    public class SelectionItemSpawner : MonoBehaviour
    {
        /// <summary>
        /// Объект заспаунен.
        /// </summary>
        public event Action onSpawned = delegate { };

        /// <summary>
        /// Последний заспауненный объект.
        /// </summary>
        public GameObject LastSpawnedObject => lastSpawnedObject;

        /// <summary>
        /// Индификатор спаунера.
        /// </summary>
        public string Id => id;

        [SerializeField] protected string id = string.Empty;

        [Header("Префабы для спауна (по умолчанию выбирается первый)")]
        [SerializeField] protected List<SelectionItemPrefab> prefabs = new List<SelectionItemPrefab>();
        [SerializeField] protected SavableSelectionInstance saveControllerInstanceContainer = default;
        [SerializeField] protected bool isAutoSpawn = false;
        [Header("Спаун префаба в заданной точке")]
        [SerializeField] protected bool isSpawnInPoint = false;
        [SerializeField] protected Transform targetSpawnPoint = default;
        protected GameObject lastSpawnedObject = default;
        protected SelectionItemPrefab currentItemPrefab = default;
        protected bool isInstanceAvailable => saveControllerInstanceContainer != null && saveControllerInstanceContainer.IsInited;
        protected Dictionary<string, GameObject> spawnedObjects = new Dictionary<string, GameObject>();
        protected GameObject currentActiveObject = default;

        /// <summary>
        /// Выполняем спаун.
        /// </summary>
        public virtual void Spawn()
        {
            if (TryInitPrefab() && currentItemPrefab.Prefab != null)
            {
                SpawnObject(transform.position, null);
                onSpawned.Invoke();
            }
        }

        /// <summary>
        /// Выполняем спаун в конкретной точке.
        /// </summary>
        /// <param name="spawnPoint"></param>
        public virtual void Spawn(Transform spawnPoint)
        {
            if (TryInitPrefab() && currentItemPrefab.Prefab != null)
            {
                SpawnObject(spawnPoint.position, spawnPoint);
                onSpawned.Invoke();
            }
        }

        protected virtual void Start()
        {
            if (isInstanceAvailable && isAutoSpawn)
            {
                saveControllerInstanceContainer.Instance.onSelectionChanged += SpawnHandler;

                SpawnHandler();
            }
        }

        protected virtual void OnDestroy()
        {
            Delegate[] delegates = onSpawned.GetInvocationList();

            if (delegates != null)
            {
                foreach (Delegate del in delegates)
                {
                    onSpawned -= (Action)del;
                }
            }

            if (isInstanceAvailable)
            {
                saveControllerInstanceContainer.Instance.onSelectionChanged -= SpawnHandler;
            }
        }

        protected void SpawnHandler()
        {
            if (isSpawnInPoint && targetSpawnPoint != null)
            {
                Spawn(targetSpawnPoint);
                return;
            }

            Spawn();
        }

        protected virtual void SpawnObject(Vector3 position, Transform parent)
        {
            string prefabId = currentItemPrefab.Id;

            if (spawnedObjects.TryGetValue(prefabId, out GameObject existingObject) && existingObject != null)
            {
                existingObject.SetActive(true);
                lastSpawnedObject = existingObject;
            }
            else
            {
                if (existingObject != null)
                    spawnedObjects.Remove(prefabId);

                GameObject newObject = parent != null ?
                    Instantiate(currentItemPrefab.Prefab, parent) :
                    Instantiate(currentItemPrefab.Prefab, position, Quaternion.identity);

                spawnedObjects[prefabId] = newObject;
                lastSpawnedObject = newObject;
            }

            if (currentActiveObject != null && currentActiveObject != lastSpawnedObject)
            {
                currentActiveObject.SetActive(false);
            }

            currentActiveObject = lastSpawnedObject;
        }

        protected bool TryInitPrefab()
        {
            if (saveControllerInstanceContainer && saveControllerInstanceContainer.IsInited && prefabs.Count > 0)
            {
                currentItemPrefab = prefabs.Find(x => x.Id == ((saveControllerInstanceContainer.Instance).GetCurrentSelection()).Id);

                return currentItemPrefab != null;
            }

            if (prefabs.Count > 0)
            {
                currentItemPrefab = prefabs[0];
            }

            return currentItemPrefab != null;
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (prefabs.Count < 1 || saveControllerInstanceContainer == null)
            {
                CDebug.LogError("Необходимо заполнить все поля!");
            }
        }
#endif
    }
}