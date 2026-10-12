namespace WinterDrift.Features.Triggers
{
    using DekDev.Extensions.Utils;

    /// <summary>
    /// Trigger for checking the direction of movement at checkpoints.
    /// </summary>
    public class LoseCheckpointTrigger : AbstractGenericTrigger<LoseCheckpoint>
    {
        public LoseCheckpoint CurrentCheckpoint => targetComponent;
    }
}