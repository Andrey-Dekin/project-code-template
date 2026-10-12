namespace WinterDrift.Features.PlayerFacade
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using DekDev.Extensions.Coroutines;
    using DekDev.Extensions.Facade;
    using UnityEngine;

    /// <summary>
    /// Drift score controller for player facade.
    /// </summary>
    public class DriftScoreController : AbstractControllerFacade<PlayerFacade>
    {
        protected CarController carController = default;
        protected DriftScoreModel driftScoreModel = default;
        protected CoroutineExtension coroutineExtension = default;

        protected bool isDrifting = false;
        protected bool wasDrifting = false;
        protected float comboTimer = 0f;
        protected float currentDriftDuration = 0f;
        protected bool isInComboWindow = false;

        protected virtual void Awake() => coroutineExtension = new CoroutineExtension(this);

        public override void InitModule()
        {
            carController = facadeInstance.Controllers.OfType<CarController>().FirstOrDefault();
            driftScoreModel = facadeInstance.Models.OfType<DriftScoreModel>().FirstOrDefault();

            if (carController != null)
            {
                carController.onDriftStateChanged += OnDriftStateChanged;
                carController.onSpeedChanged += OnSpeedChanged;
            }
        }

        public override void Dispose()
        {
            DisposeData();

            base.Dispose();
        }

        protected virtual void OnDisable()
        {
            DisposeData();

            coroutineExtension.Coroutine = null;
        }

        protected virtual void DisposeData()
        {
            if (carController != null)
            {
                carController.onDriftStateChanged -= OnDriftStateChanged;
                carController.onSpeedChanged -= OnSpeedChanged;
            }

            if (driftScoreModel != null)
            {
                driftScoreModel.ResetDriftData();
            }
        }

        protected void OnDriftStateChanged()
        {
            wasDrifting = isDrifting;
            isDrifting = carController.IsDrifting;

            HandleDriftStateChange();
        }

        protected void OnSpeedChanged()
        {
            if (isDrifting && !carController.IsDrifting)
            {
                isDrifting = false;
                HandleDriftStateChange();
            }
        }

        protected void HandleDriftStateChange()
        {
            if (isDrifting && !wasDrifting)
            {
                if (isInComboWindow)
                {
                    isInComboWindow = false;
                    comboTimer = 0f;
                    
                    coroutineExtension.Coroutine = StartCoroutine(DriftScoringCoroutine());
                }
                else
                {
                    StartDrift();
                }
            }
            else if (!isDrifting && wasDrifting)
            {
                isInComboWindow = true;
                comboTimer = driftScoreModel.ComboTimeWindow;
                coroutineExtension.Coroutine = StartCoroutine(ComboCheckCoroutine());
            }
        }

        protected void StartDrift()
        {
            driftScoreModel.StartDrift();
            currentDriftDuration = 0f;
            coroutineExtension.Coroutine = StartCoroutine(DriftScoringCoroutine());
        }

        protected void EndDrift()
        {
            driftScoreModel.EndDrift();
            currentDriftDuration = 0f;
            isInComboWindow = false;
        }

        protected IEnumerator DriftScoringCoroutine()
        {
            var waitForSeconds = new WaitForSeconds(driftScoreModel.ScoreUpdateInterval);

            while (isDrifting && isActiveAndEnabled)
            {
                yield return waitForSeconds;

                if (!isDrifting) break;

                currentDriftDuration += driftScoreModel.ScoreUpdateInterval;
                driftScoreModel.UpdateDriftTime(driftScoreModel.ScoreUpdateInterval);

                float timeMultiplier = CalculateTimeMultiplier(currentDriftDuration);
                driftScoreModel.SetMultiplier(timeMultiplier);

                float angleScore = CalculateAngleScore();
                
                float finalScore = angleScore * timeMultiplier * driftScoreModel.ScoreUpdateInterval;
                driftScoreModel.AddScore(finalScore);
            }
        }

        protected IEnumerator ComboCheckCoroutine()
        {
            while (comboTimer > 0f && !isDrifting && isActiveAndEnabled)
            {
                comboTimer -= Time.deltaTime;
                yield return null;
            }

            if (!isDrifting && comboTimer <= 0f)
            {
                EndDrift();
                isInComboWindow = false;
            }
        }

        protected float CalculateTimeMultiplier(float driftTime)
        {
            float timeRatio = Mathf.Clamp01(driftTime / driftScoreModel.TimeToMaxMultiplier);
            return 1f + (driftScoreModel.MaxTimeMultiplier - 1f) * timeRatio;
        }

        protected float CalculateAngleScore()
        {
            float absAngle = Mathf.Abs(carController.DriftAngle);
            float normalizedAngle = Mathf.Clamp01(absAngle / 1.5f);
            return driftScoreModel.AngleMultiplierCurve.Evaluate(normalizedAngle) * 10f;
        }
    }
}