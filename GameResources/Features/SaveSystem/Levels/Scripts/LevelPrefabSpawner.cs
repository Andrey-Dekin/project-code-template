namespace WinterDrift.Features.SaveSystem.Levels
{
    /// <summary>
    /// Level prefab spawner.
    /// </summary>
    public class LevelPrefabSpawner : AbstractLevelItemSpawner
    {
        protected override void InitializePrefab()
        {
            if (currentLevelData.LevelPrefab != null)
            {
                prefab = currentLevelData.LevelPrefab;
            }
        }
    }
}