namespace DekDev.Extensions.Timers
{
    using System;
    using System.Collections;
    using UnityEngine;
    using DekDev.Extensions.Coroutines;

    /// <summary>
    /// Timer extension using coroutines.
    /// </summary>
    public class CoroutineTimerExtension
    {
        public event Action onStarted = delegate { };
        public event Action onStopped = delegate { };
        public event Action onFinished = delegate { };
        public event Action onSecondPassed = delegate { };

        /// <summary>
        /// Current increment/countdown timer time.
        /// </summary>
        public TimeSpan CurrentTime { get; protected set; } = default;
        
        /// <summary>
        /// Is timer running.
        /// </summary>
        public bool IsRunning { get; protected set; } = false;

        protected readonly CoroutineExtension coroutineExtension;
        protected bool isCountdownMode = false;

        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="owner"></param>
        public CoroutineTimerExtension(MonoBehaviour owner)
        {
            coroutineExtension = new CoroutineExtension(owner);
        }

        /// <summary>
        /// Start increment timer.
        /// </summary>
        public virtual void Start()
        {
            if (IsRunning) return;

            IsRunning = true;
            isCountdownMode = false;
            CurrentTime = TimeSpan.Zero;

            onStarted();

            coroutineExtension.Coroutine = coroutineExtension.Owner.StartCoroutine(RunTimer());
        }

        /// <summary>
        /// Start countdown timer.
        /// </summary>
        /// <param name="duration"></param>
        public virtual void StartCountdown(TimeSpan duration)
        {
            if (IsRunning) return;

            CurrentTime = duration;
            IsRunning = true;
            isCountdownMode = true;

            onStarted();

            coroutineExtension.Coroutine = coroutineExtension.Owner.StartCoroutine(RunCountdown());
        }

        /// <summary>
        /// Stop timer.
        /// </summary>
        public virtual void Stop()
        {
            if (!IsRunning) return;

            IsRunning = false;

            coroutineExtension.Coroutine = null;

            onStopped();
        }

        /// <summary>
        /// Finish countdown timer.
        /// </summary>
        public virtual void Complete()
        {
            if (!IsRunning) return;

            IsRunning = false;

            coroutineExtension.Coroutine = null;

            if (isCountdownMode)
            {
                CurrentTime = TimeSpan.Zero;
            }

            onFinished();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public virtual void Dispose()
        {
            Stop();

            coroutineExtension.Coroutine = null;
        }

        protected virtual IEnumerator RunTimer()
        {
            var previousSecond = TimeSpan.Zero;

            while (IsRunning)
            {
                yield return null;
                
                CurrentTime = CurrentTime.Add(TimeSpan.FromSeconds(Time.deltaTime));

                if ((int)CurrentTime.TotalSeconds > (int)previousSecond.TotalSeconds)
                {
                    previousSecond = CurrentTime;
                    onSecondPassed();
                }
            }
        }

        protected virtual IEnumerator RunCountdown()
        {
            var previousSecond = CurrentTime;

            while (IsRunning && CurrentTime > TimeSpan.Zero)
            {
                yield return null;
                
                CurrentTime = CurrentTime.Subtract(TimeSpan.FromSeconds(Time.deltaTime));

                if ((int)CurrentTime.TotalSeconds < (int)previousSecond.TotalSeconds)
                {
                    previousSecond = CurrentTime;
                    onSecondPassed();
                }
            }

            if (CurrentTime <= TimeSpan.Zero && IsRunning)
            {
                Complete();
            }
        }
    }
}