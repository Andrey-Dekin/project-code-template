namespace WinterDrift.Features.PlayerFacade
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using DekDev.Extensions.Coroutines;
    using DekDev.Extensions.Facade;
    using UnityEngine;

    /// <summary>
    /// Car controller for player facade.
    /// </summary>
    [RequireComponent(typeof(RCC_CarControllerV3))]
    public class CarController : AbstractControllerFacade<PlayerFacade>
    {
        protected const float UPDATE_TIME = 0.1f;

        /// <summary>
        /// Car start drift event.
        /// </summary>
        public event Action onDriftStateChanged = delegate { };

        /// <summary>
        /// Car speed change.
        /// </summary>
        public event Action onSpeedChanged = delegate { };

        /// <summary>
        /// Drift angle change.
        /// </summary>
        public event Action onDriftAngleChanged = delegate { };

        /// <summary>
        /// Availability of player input for vehicle control changed.
        /// </summary>
        public event Action onControlAvailabilityChanged = delegate { };

        /// <summary>
        /// Current dift state.
        /// </summary>
        public bool IsDrifting
        {
            get => isDrifting;

            protected set
            {
                _isNewDriftState = value && CurrentSpeed >= minDriftSpeed;

                if (_isNewDriftState != isDrifting)
                {
                    isDrifting = _isNewDriftState;
                    onDriftStateChanged();
                }
            }
        }

        /// <summary>
        /// Current drift angle.
        /// </summary>
        public float DriftAngle
        {
            get => driftAngle;

            protected set
            {
                if (value != driftAngle)
                {
                    driftAngle = value;
                    onDriftAngleChanged();
                }
            }
        }

        /// <summary>
        /// Current car speed.
        /// </summary>
        public float CurrentSpeed
        {
            get => currentSpeed;

            protected set
            {
                if (currentSpeed != value)
                {
                    currentSpeed = value;
                    onSpeedChanged();
                }
            }
        }

        /// <summary>
        /// Availability of player input for vehicle control.
        /// </summary>
        public bool IsControlAvailable
        {
            get => isControlAvailable;

            protected set
            {
                if (isControlAvailable != value)
                {
                    isControlAvailable = value;
                    onControlAvailabilityChanged();
                }
            }
        }

        [SerializeField, Min(0f)] protected float minDriftSpeed = 15f;
        protected RCC_CarControllerV3 carControllerRCC = default;
        protected bool isDrifting = false;
        protected float driftAngle = 0f;
        protected float currentSpeed = 0f;
        protected bool isControlAvailable = false;
        protected CoroutineExtension coroutineExtension = default;
        protected WaitForSeconds waitForSeconds = new WaitForSeconds(UPDATE_TIME);
        private bool _isNewDriftState = false;

        public override void InitModule()
        {
            coroutineExtension.Coroutine = StartCoroutine(UpdateCoroutine());
        }


        public override void Dispose()
        {
            base.Dispose();

            coroutineExtension = null;
        }

        /// <summary>
        /// Set car control available state.
        /// </summary>
        /// <param name="state"></param>
        public void SetControlAvailableState(bool state)
        {
            if (carControllerRCC != null)
            {
                carControllerRCC.SetCanControl(state);
            }
        }

        protected virtual void Awake()
        {
            carControllerRCC = GetComponent<RCC_CarControllerV3>();
            coroutineExtension = new CoroutineExtension(this);
        }

        protected virtual void OnDisable()
        {
            coroutineExtension.Coroutine = null;
        }

        protected IEnumerator UpdateCoroutine()
        {
            while (isActiveAndEnabled)
            {
                yield return waitForSeconds;

                IsDrifting = carControllerRCC.driftingNow;
                DriftAngle = carControllerRCC.driftAngle;
                CurrentSpeed = carControllerRCC.speed;
                IsControlAvailable = carControllerRCC.canControl;
            }
        }
    }
}