namespace DekDev.Extensions.Tweeners
{
    using System.Collections;
    using DekDev.Extensions.Coroutines;
    using UnityEngine;
    using UnityEngine.Events;
    using DekDev.CDebug;

    /// <summary>
    /// Abstract base class for tweening properties with optional ping-pong effect.
    /// </summary>
    public abstract class AbstractTweener : MonoBehaviour
    {
        [SerializeField] protected bool isPlayOnEnable = true;
        [SerializeField, Min(0f)] protected float delay = 1f;
        [SerializeField, Min(0f)] protected float duration = 1f;
        [SerializeField] protected AnimationCurve animationCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField] protected bool pingPong = false;
        [SerializeField] protected bool isUnscaledTime = false;
        [SerializeField] protected bool resetOnDisable = true;

        [Header("Events")]
        [SerializeField] protected UnityEvent onStart = default;
        [SerializeField] protected UnityEvent onComplete = default;
        [SerializeField] protected UnityEvent onPingPongCycleComplete = default;

        protected CoroutineExtension coroutineExtension = default;
        protected float lerpDelta = 0f;

        /// <summary>
        /// Start forward the animation manually.
        /// </summary>
        [ContextMenu("Play tween forward")]
        public virtual void PlayForward()
        {
            if (!isActiveAndEnabled)
            {
                CDebug.LogError("Tweener disabled and can't play");
                return;
            }

            StopAnimation();
            coroutineExtension.Coroutine = StartCoroutine(TweenRoutine());
        }

        /// <summary>
        /// Start backward the animation manually.
        /// </summary>
        [ContextMenu("Play tween backward")]
        public virtual void PlayBackward()
        {
            if (!isActiveAndEnabled)
            {
                CDebug.LogError("Tweener disabled and can't play");
                return;
            }

            StopAnimation();
            coroutineExtension.Coroutine = StartCoroutine(TweenRoutine(false));
        }

        /// <summary>
        /// Stop the animation.
        /// </summary>
        [ContextMenu("Stop tween")]
        public virtual void StopAnimation()
        {
            if (coroutineExtension != null)
            {
                coroutineExtension.Coroutine = null;
            }
        }

        /// <summary>
        /// Restart the animation.
        /// </summary>
        [ContextMenu("Restart tween")]
        public virtual void RestartAnimation()
        {
            StopAnimation();

            if (isActiveAndEnabled)
            {
                coroutineExtension.Coroutine = StartCoroutine(TweenRoutine());
            }
        }

        protected virtual void Awake()
        {
            coroutineExtension = new CoroutineExtension(this);
        }

        protected virtual void OnEnable()
        {
            if (isPlayOnEnable)
            {
                coroutineExtension.Coroutine = StartCoroutine(TweenRoutine());
            }
        }

        protected virtual void OnDisable()
        {
            if (coroutineExtension != null)
            {
                coroutineExtension.Coroutine = null;
            }

            if (resetOnDisable)
            {
                ResetToInitialState();
            }
        }

        protected virtual IEnumerator TweenRoutine(bool isForward = true)
        {
            if (delay > 0f)
            {
                if (isUnscaledTime)
                {
                    yield return new WaitForSecondsRealtime(delay);
                }
                else
                {
                    yield return new WaitForSeconds(delay);
                }
            }

            onStart?.Invoke();

            if (pingPong)
            {
                while (isActiveAndEnabled)
                {
                    yield return TweenToTarget(GetInitialValue(), GetTargetValue());
                    yield return TweenToTarget(GetTargetValue(), GetInitialValue());

                    onPingPongCycleComplete?.Invoke();
                }
            }
            else
            {
                if (isForward)
                {
                    yield return TweenToTarget(GetInitialValue(), GetTargetValue());
                }
                else
                {
                    yield return TweenToTarget(GetTargetValue(), GetInitialValue());
                }

                onComplete?.Invoke();
            }
        }

        protected virtual IEnumerator TweenToTarget(Vector3 fromValue, Vector3 toValue)
        {
            float elapsed = 0f;

            while (isActiveAndEnabled && elapsed < duration)
            {
                elapsed += isUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                lerpDelta = Mathf.Clamp01(elapsed / duration);

                float curvedValue = animationCurve.Evaluate(lerpDelta);
                ApplyTweenValue(Vector3.Lerp(fromValue, toValue, curvedValue));

                yield return null;
            }

            if (isActiveAndEnabled)
            {
                ApplyTweenValue(toValue);
            }
        }

        protected abstract Vector3 GetInitialValue();
        protected abstract Vector3 GetTargetValue();
        protected abstract void ApplyTweenValue(Vector3 value);
        protected abstract void ResetToInitialState();
    }
}