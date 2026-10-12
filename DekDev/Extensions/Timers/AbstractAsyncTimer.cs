namespace DekDev.Extensions.Timers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Abstract async timer class.
    /// </summary>
    public abstract class AbstractAsyncTimer
    {
        public event Action onStarted = delegate { };
        public event Action onStopped = delegate { };
        public event Action onFinished = delegate { };
        public event Action onSecondPassed = delegate { };

        /// <summary>
        /// Current increment/countdown timer time.
        /// </summary>
        public TimeSpan CurrentTime { get; protected set; } = default;
        public bool IsRunning { get; protected set; } = false;

        protected readonly SynchronizationContext unityContext = default;
        protected CancellationTokenSource cancellationToken = default;

        protected AbstractAsyncTimer()
        {
            unityContext = SynchronizationContext.Current;
            cancellationToken = new CancellationTokenSource();
        }

        /// <summary>
        /// Start increment timer.
        /// </summary>
        public virtual void Start()
        {
            if (IsRunning) return;

            IsRunning = true;
            cancellationToken = new CancellationTokenSource();

            onStarted();

            RunTimer(cancellationToken.Token);
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
            cancellationToken = new CancellationTokenSource();

            onStarted();

            RunCountdown(cancellationToken.Token);
        }

        /// <summary>
        /// Stop timer.
        /// </summary>
        public virtual void Stop()
        {
            if (!IsRunning) return;

            cancellationToken.Cancel();
            IsRunning = false;

            onStopped();
        }

        /// <summary>
        /// Finish countdown timer.
        /// </summary>
        public virtual void Complete()
        {
            if (!IsRunning) return;

            cancellationToken.Cancel();
            IsRunning = false;
            CurrentTime = TimeSpan.Zero;

            onFinished();
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public virtual void Dispose()
        {
            cancellationToken?.Cancel();
            cancellationToken?.Dispose();
        }

        protected void InvokeInUnityContext(Action action)
        {
            unityContext?.Post(_ => action(), null);
        }

        protected void HandleSecondPassed()
        {
            InvokeInUnityContext(() => onSecondPassed());
        }

        protected void OnTimerStartAction() => onStarted();

        protected void OnTimerStopAction() => onStopped();

        protected void OnTimerFinishedAction() => onFinished();

        /// <summary>
        /// Run async increment timer.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected abstract Task RunTimer(CancellationToken token);

        /// <summary>
        /// Run async countdown timer.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        protected abstract Task RunCountdown(CancellationToken token);
    }
}