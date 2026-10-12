namespace DekDev.Extensions.Spawners
{
    using UnityEngine;

/// <summary>
/// Item Pool, активация элемента из пула и обратно
/// </summary>
    public class ItemPool : MonoBehaviour
    {
        protected PoolSpawner spawner = default;

        /// <summary>
        /// Инициализация
        /// </summary>
        /// <param name="_spawner"></param>
        public void Init(PoolSpawner _spawner)
        {
            spawner = _spawner;
        }

        protected virtual void OnDisable()
        {
            if (spawner == null) return;
            spawner.ReturnToPool(this);
        }
    }
}