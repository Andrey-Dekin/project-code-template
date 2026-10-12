namespace DekDev.Extensions.Spawners
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using DekDev.Extensions.Utils;
    using DekDev.Extensions.Objects;

    /// <summary>
    /// Пул спаунер
    /// </summary>
    public class PoolSpawner : CyclicSpawner
    {
        /// <summary>
        /// Количество объектов в пуле
        /// </summary>
        public int ItemsCount => items.Count;
        public event Action onItemsCountChanged = delegate { };
        [SerializeField] protected List<ItemPool> items = new();
        [SerializeField, Min(0)] protected int countPrewarm = 3;
        protected ItemPool tempItem = default;

        /// <summary>
        /// Изменить количество предзаготовленных итемов, валидно для вызова в Awake, позднее вызывать уже нет смысла
        /// </summary>
        /// <param name="_newPrewarm"></param>
        public void ChangePrewarm(int _newPrewarm) => countPrewarm = _newPrewarm;

        protected override void Start()
        {
            for (int i = 0; i < countPrewarm; i++)
            {
                base.Spawn();
                tempItem = LastSpawnedObject.GetComponent<ItemPool>();
                tempItem.Init(this);
                tempItem.gameObject.SetActive(false);
            }

            base.Start();
        }

        /// <summary>
        /// Спаун с помощью пула
        /// </summary>
        [ContextMenu("Test Spawn")]
        public override void Spawn()
        {
            if (items.Count <= 0)
            {
                base.Spawn();
                ItemPool itemPool = LastSpawnedObject.GetComponent<ItemPool>();
                itemPool.Init(this);
            }
            else
            {
                items[items.Count - 1].gameObject.SetActive(true);
                items[items.Count - 1].gameObject.ResetLocalPosition(gameObject);
                lastSpawnedObject = items[items.Count - 1].gameObject;
                items.RemoveAt(items.Count - 1);
                onItemsCountChanged();
            }
        }

        public override void Spawn(Transform spawnPoint)
        {
            Spawn();
            lastSpawnedObject.ResetLocalPosition(spawnPoint.gameObject);
        }

        /// <summary>
        /// Вернуть в пул
        /// </summary>
        public void ReturnToPool(ItemPool itemPool)
        {
            items.Add(itemPool);
            itemPool.gameObject.SetActive(false);
            onItemsCountChanged();
        }

        /// <summary>
        /// Саун со сменой префаба в указанной точке
        /// </summary>
        /// <param name="newPrefab"></param>
        /// <param name="spawnPoint"></param>
        public virtual void Spawn(GameObject newPrefab, Transform spawnPoint)
        {
            if (prefab != newPrefab)
            {
                prefab = newPrefab;
            }
            Spawn(spawnPoint);
        }
    }
}