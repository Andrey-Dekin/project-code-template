namespace WinterDrift.Features.SaveSystem.Levels
{
    using DekDev.CDebug;
    using DekDev.Extensions.Spawners;
    using DekDev.SaveSystem;
    using UnityEngine;

    /// <summary>
    /// Base selected level items spawner.
    /// </summary>
    public abstract class AbstractLevelItemSpawner : BaseSpawner
    {
        [SerializeField] protected SavableSelectionInstance savableInstanceContainer = default;
        [SerializeField] protected Transform spawnPoint = default;
        protected bool isInstanceAvailable => savableInstanceContainer != null && savableInstanceContainer.IsInited;
        protected BaseLevelItem currentLevelData = default;

        protected override void Start()
        {
            if (isAutoSpawn && isInstanceAvailable && TryInitialize())
            {
                Spawn(spawnPoint);
            }
        }

        /// <summary>
        /// Initialize prefab with return bool.
        /// </summary>
        /// <returns></returns>
        protected virtual bool TryInitialize()
        {
            prefab = null;

            currentLevelData = savableInstanceContainer.Instance.GetCurrentSelection() as BaseLevelItem;

            if (currentLevelData != null)
            {
                InitializePrefab();
            }
            else
            {
                CDebug.LogError("Can't find prefab!");
            }

            return prefab != null;
        }

        /// <summary>
        /// Initialize prefab for spawn.
        /// </summary>
        protected abstract void InitializePrefab();
    }
}