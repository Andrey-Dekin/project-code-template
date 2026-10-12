namespace WinterDrift.Features.Triggers
{
    using DekDev.Extensions.Utils;

    /// <summary>
    /// Trigger for checkppoints.
    /// </summary>
    public class CheckpointTrigger : AbstractGenericTrigger<Checkpoint>
    {
        /// <summary>
        /// Current triggered zone.
        /// </summary>
        public Checkpoint CurrentZone => targetComponent;
    }
}