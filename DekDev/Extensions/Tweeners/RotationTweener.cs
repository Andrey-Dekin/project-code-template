namespace DekDev.Extensions.Tweeners
{
    using UnityEngine;

    /// <summary>
    /// Rotate object tweening with optional ping-pong effect.
    /// </summary>
    public class RotationTweener : AbstractTweener
    {
        [SerializeField] protected Vector3 from = Vector3.zero;
        [SerializeField] protected Vector3 to = new Vector3(0f, 100f, 0f);

        protected Vector3 initialValue = Vector3.zero;

        protected override void Awake()
        {
            base.Awake();
            initialValue = transform.localEulerAngles;
        }

        protected override Vector3 GetInitialValue() => from;
        protected override Vector3 GetTargetValue() => to;

        protected override void ApplyTweenValue(Vector3 value)
        {
            transform.localEulerAngles = value;
        }

        protected override void ResetToInitialState()
        {
            transform.localEulerAngles = initialValue;
        }
    }
}