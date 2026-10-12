namespace WinterDrift.Features.SaveSystem.Levels
{
    /// <summary>
    /// Selected level track spawner.
    /// </summary>
    public class LevelTrackSpawner : AbstractLevelItemSpawner
    {
        protected override void InitializePrefab()
        {
            if (currentLevelData.Track != null && currentLevelData.Track.Prefab != null)
            {
                prefab = currentLevelData.Track.Prefab;
            }
        }
    }
}