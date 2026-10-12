namespace WinterDrift.Features.PlayerFacade.Views
{
    using System.Linq;
    using DekDev.Extensions.Coroutines;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Speed fill indicator view with lerp animation using coroutine.
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class SpeedFillView : PlayerFacadeProvider
    {
        [SerializeField, Min(1f)] protected float maxSpeed = 50f;
        [SerializeField, Min(0f)] protected float lerpDuration = 0.3f;
        protected CarController carController = default;
        protected Image fillImage = default;
        protected CoroutineExtension coroutineExtension = default;
        protected WaitForSeconds waitForSeconds = default;
        protected float targetFillAmount = 0f;
        protected float currentFillAmount = 0f;
        protected bool isLerping = false;

        protected virtual void Awake()
        {
            fillImage = GetComponent<Image>();
            coroutineExtension = new CoroutineExtension(this);
        }

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            carController = facade.Controllers.OfType<CarController>().FirstOrDefault();

            if (carController != null)
            {
                carController.onSpeedChanged += UpdateFillTarget;
                currentFillAmount = CalculateNormalizedSpeed();
                fillImage.fillAmount = currentFillAmount;
            }
        }

        protected override void OnInstanceDisposed()
        {
            base.OnInstanceDisposed();
            Unsubscribe();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            Unsubscribe();
        }

        protected virtual void UpdateFillTarget()
        {
            if (carController == null || fillImage == null) return;

            targetFillAmount = CalculateNormalizedSpeed();

            if (!Mathf.Approximately(targetFillAmount, currentFillAmount))
            {
                if (coroutineExtension.Coroutine == null)
                {
                    coroutineExtension.Coroutine = StartCoroutine(LerpCoroutine());
                }
            }
        }

        protected virtual System.Collections.IEnumerator LerpCoroutine()
        {
            float startTime = Time.time;
            float startValue = currentFillAmount;
            float elapsedTime = 0f;
            float t = 0f;

            while (isActiveAndEnabled && !Mathf.Approximately(currentFillAmount, targetFillAmount))
            {
                elapsedTime = Time.time - startTime;
                t = Mathf.Clamp01(elapsedTime / lerpDuration);

                currentFillAmount = Mathf.Lerp(startValue, targetFillAmount, t);
                fillImage.fillAmount = currentFillAmount;

                yield return waitForSeconds;

                if (!Mathf.Approximately(targetFillAmount, CalculateNormalizedSpeed()))
                {
                    targetFillAmount = CalculateNormalizedSpeed();
                    startTime = Time.time;
                    startValue = currentFillAmount;
                }
            }

            currentFillAmount = targetFillAmount;
            fillImage.fillAmount = currentFillAmount;

            coroutineExtension.Coroutine = null;
        }

        protected virtual float CalculateNormalizedSpeed()
        {
            if (carController == null) return 0f;

            return Mathf.Clamp01(carController.CurrentSpeed / maxSpeed);
        }

        protected virtual void Unsubscribe()
        {
            if (carController != null)
            {
                carController.onSpeedChanged -= UpdateFillTarget;
            }

            if (coroutineExtension != null)
            {
                coroutineExtension.Coroutine = null;
            }
        }
    }
}