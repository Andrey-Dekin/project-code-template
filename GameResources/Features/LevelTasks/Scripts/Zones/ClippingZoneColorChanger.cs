namespace WinterDrift.Features.LevelTasks
{
    using System.Linq;
    using System.Collections;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;
    using DekDev.Extensions.Coroutines;

    /// <summary>
    /// Change clipping zone material color of drift status.
    /// </summary>
    public class ClippingZoneColorChanger : PlayerFacadeProvider
    {
        [SerializeField] protected Color activeColor = Color.green;
        [SerializeField] protected Color inactiveColor = Color.yellow;
        [SerializeField] protected float colorTransitionDuration = 0.3f;
        [SerializeField] protected Material material = default;
        protected CarController carController = default;
        protected CoroutineExtension colorChangeCoroutine = default;
        protected Color targetColor = default;
        protected Color initColor = default;

        protected virtual void Awake()
        {
            colorChangeCoroutine = new CoroutineExtension(this);

            initColor = material.color;
        }

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            if (carController == null)
            {
                carController = facade.Controllers.OfType<CarController>().FirstOrDefault();
            }

            targetColor = material.color;
            Subscribe();
        }

        protected override void OnInstanceDisposed()
        {
            base.OnInstanceDisposed();
            Unsubscribe();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            material.color = initColor;

            Unsubscribe();
        }

        protected virtual void OnDriftStateChanged()
        {
            targetColor = carController.IsDrifting ? activeColor : inactiveColor;

            colorChangeCoroutine.Coroutine = StartCoroutine(ColorTransitionRoutine());
        }

        protected IEnumerator ColorTransitionRoutine()
        {
            Color startColor = material.color;
            float elapsedTime = 0f;

            while (isActiveAndEnabled && elapsedTime < colorTransitionDuration)
            {
                elapsedTime += Time.deltaTime;
                material.color = Color.Lerp(startColor, targetColor, elapsedTime / colorTransitionDuration);
                
                yield return null;
            }

            material.color = targetColor;
        }

        protected virtual void Subscribe()
        {
            if (carController != null)
            {
                carController.onDriftStateChanged += OnDriftStateChanged;
            }
        }

        protected virtual void Unsubscribe()
        {
            if (carController != null)
            {
                carController.onDriftStateChanged -= OnDriftStateChanged;
            }

            colorChangeCoroutine.Coroutine = null;
        }
    }
}