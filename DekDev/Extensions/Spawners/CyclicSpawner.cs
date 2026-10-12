namespace DekDev.Extensions.Spawners
{
    using System.Collections;
    using UnityEngine;

    /// <summary>
    /// Цикличный спаун
    /// </summary>
    public class CyclicSpawner : BaseSpawner
    {
        [SerializeField, Min(1f)] private float timeBetweenSpawns = 1f;

        protected override void Start()
        {
            base.Start();
            if (isAutoSpawn)
            {
                StartCoroutine(SpawnCycle());
            }
        }

        protected IEnumerator SpawnCycle()
        {
            while (isActiveAndEnabled)
            {
                yield return new WaitForSeconds(timeBetweenSpawns);

                Spawn();
            }
        }
    }
}
