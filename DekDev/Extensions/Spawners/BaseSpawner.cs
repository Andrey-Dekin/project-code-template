namespace DekDev.Extensions.Spawners
{
    using System;
    using UnityEngine;

    /// <summary>
    /// Базовый спаунер префаба.
    /// </summary>
    public class BaseSpawner : MonoBehaviour
    {
        /// <summary>
        /// Объект заспаунен.
        /// </summary>
        public event Action onSpawned = delegate { };

        /// <summary>
        /// Префаб для спауна.
        /// </summary>
        public GameObject Prefab => prefab;

        /// <summary>
        /// Последний заспауненный объект.
        /// </summary>
        public GameObject LastSpawnedObject => lastSpawnedObject;

        /// <summary>
        /// Индификатор спаунера.
        /// </summary>
        public string Id => id;

        [SerializeField] protected GameObject prefab = default;
        [SerializeField] protected bool isAutoSpawn = default;
        [SerializeField] protected string id = string.Empty;
        protected GameObject lastSpawnedObject = default;

        /// <summary>
        /// Выполняем спаун
        /// </summary>
        public virtual void Spawn()
        {
            lastSpawnedObject = Instantiate(prefab, transform.position, Quaternion.identity);
            onSpawned.Invoke();
        }

        /// <summary>
        /// Выполняем спаун в конкретной точке
        /// </summary>
        /// <param name="spawnPoint"></param>
        public virtual void Spawn(Transform spawnPoint)
        {
            lastSpawnedObject = Instantiate(prefab, spawnPoint);
            onSpawned.Invoke();
        }

        protected virtual void Start()
        {
            if (isAutoSpawn)
            {
                Spawn();
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
        }
    }
}