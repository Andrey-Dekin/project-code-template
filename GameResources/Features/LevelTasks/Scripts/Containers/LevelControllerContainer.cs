namespace WinterDrift.Features.LevelTasks
{
    using DekDev.Extensions.Containers;
    using UnityEngine;

    /// <summary>
    /// Container of level controller component.
    /// </summary>
    [CreateAssetMenu(fileName = nameof(LevelControllerContainer), menuName = "WinterDrift/Features/LevelTasks/Containers/" + nameof(LevelControllerContainer))]
    public class LevelControllerContainer : AbstractComponentContainer<LevelController>
    {
    }
}