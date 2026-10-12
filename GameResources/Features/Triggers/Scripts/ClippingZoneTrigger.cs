namespace WinterDrift.Features.Triggers
{
    using DekDev.Extensions.Utils;

    /// <summary>
    /// Trigger for clipping zone.
    /// </summary>
    public class ClippingZoneTrigger : AbstractGenericTrigger<ClippingZone>
    {
        /// <summary>
        /// Current triggered zone.
        /// </summary>
        public ClippingZone CurrentZone => targetComponent;
    }
}