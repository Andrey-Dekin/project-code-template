namespace DekDev.Extensions.Tweeners
{
    using UnityEngine;

    /// <summary>
    /// Position object tweening with optional ping-pong effect.
    /// </summary>
    public class PositionTweener : AbstractTweener
    {
        [SerializeField] protected Vector3 from = Vector3.zero;
        [SerializeField] protected Vector3 to = new Vector3(0f, 5f, 0f);

        protected Vector3 initialValue = Vector3.zero;

        protected override void Awake()
        {
            base.Awake();
            initialValue = transform.position;
        }

        protected override Vector3 GetInitialValue() => from;
        protected override Vector3 GetTargetValue() => to;

        protected override void ApplyTweenValue(Vector3 value)
        {
            transform.position = value;
        }

        protected override void ResetToInitialState()
        {
            transform.position = initialValue;
        }
    }
}