namespace WinterDrift.Features.PlayerFacade
{
    using DekDev.Extensions.Facade;
    using UnityEngine;
    using System;

    /// <summary>
    /// Drift scoring model for player facade.
    /// </summary>
    [CreateAssetMenu(fileName = "New DriftScoreModel", menuName = "WinterDrift/Features/PlayerFacade/Models/" + nameof(DriftScoreModel))]
    public class DriftScoreModel : AbstractModelFacade<PlayerFacade>
    {
        [Header("Drift Scoring Settings")]
        [SerializeField, Range(0.1f, 5f)] private float _scoreUpdateInterval = 0.1f;
        [SerializeField, Range(0.1f, 2f)] private float _comboTimeWindow = 0.5f;
        
        [Header("Angle Multiplier Curve")]
        [SerializeField] private AnimationCurve _angleMultiplierCurve = AnimationCurve.Linear(0f, 1f, 1.5f, 3f);
        
        [Header("Time Multiplier Settings")]
        [SerializeField, Range(1f, 5f)] private float _maxTimeMultiplier = 3f;
        [SerializeField, Range(1f, 10f)] private float _timeToMaxMultiplier = 5f;

        // Current session values
        public float CurrentDriftScore { get; private set; }
        public float TotalDriftScore { get; private set; }
        public float CurrentDriftTime { get; private set; }
        public float TotalDriftTime { get; private set; }
        public float CurrentMultiplier { get; private set; } = 1f;

        public event Action OnDriftScoreUpdated = delegate { };
        public event Action OnTotalScoreUpdated = delegate { };
        public event Action OnDriftTimeUpdated = delegate { };
        public event Action OnTotalTimeUpdated = delegate { };
        public event Action OnMultiplierUpdated = delegate { };
        public event Action OnDriftStarted = delegate { };
        public event Action OnDriftEnded = delegate { };

        /// <summary>
        /// Time to change the direction of the car's movement.
        /// </summary>
        public float ComboTimeWindow => _comboTimeWindow;
        public float ScoreUpdateInterval => _scoreUpdateInterval;
        public AnimationCurve AngleMultiplierCurve => _angleMultiplierCurve;
        public float MaxTimeMultiplier => _maxTimeMultiplier;
        public float TimeToMaxMultiplier => _timeToMaxMultiplier;

        public void AddScore(float score)
        {
            CurrentDriftScore += score;
            TotalDriftScore += score;
            OnDriftScoreUpdated();
            OnTotalScoreUpdated();
        }

        public void UpdateDriftTime(float deltaTime)
        {
            CurrentDriftTime += deltaTime;
            TotalDriftTime += deltaTime;
            OnDriftTimeUpdated();
            OnTotalTimeUpdated();
        }

        public void SetMultiplier(float multiplier)
        {
            CurrentMultiplier = Mathf.Clamp(multiplier, 1f, _maxTimeMultiplier);
            OnMultiplierUpdated();
        }

        public void StartDrift()
        {
            OnDriftStarted();
        }

        public void EndDrift()
        {
            CurrentDriftScore = 0f;
            CurrentDriftTime = 0f;
            CurrentMultiplier = 1f;
            OnDriftEnded();
        }

        public void ResetDriftData()
        {
            CurrentDriftScore = 0f;
            TotalDriftScore = 0f;
            CurrentDriftTime = 0f;
            TotalDriftTime = 0f;
            CurrentMultiplier = 1f;
            OnDriftScoreUpdated();
            OnDriftTimeUpdated();
            OnMultiplierUpdated();
        }
    }
}