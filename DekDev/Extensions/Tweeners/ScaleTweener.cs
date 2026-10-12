namespace DekDev.Extensions.Tweeners
{
    using UnityEngine;

    /// <summary>
    /// Scale object tweening with optional ping-pong effect.
    /// </summary>
    public class ScaleTweener : AbstractTweener
    {
        [SerializeField] protected Vector3 from = Vector3.one;
        [SerializeField] protected Vector3 to = new Vector3(1.5f, 1.5f, 1.5f);

        protected Vector3 initialValue = Vector3.zero;

        protected override void Awake()
        {
            base.Awake();
            initialValue = transform.localScale;
        }

        protected override Vector3 GetInitialValue() => from;
        protected override Vector3 GetTargetValue() => to;

        protected override void ApplyTweenValue(Vector3 value)
        {
            transform.localScale = value;
        }

        protected override void ResetToInitialState()
        {
            transform.localScale = initialValue;
        }
    }
}