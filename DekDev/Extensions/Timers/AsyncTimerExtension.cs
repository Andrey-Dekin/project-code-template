namespace DekDev.Extensions.Timers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using UnityEngine;

    /// <summary>
    /// Extension for increment or countdown timer.
    /// </summary>
    public class AsyncTimerExtension : AbstractAsyncTimer
    {
        /// <summary>
        /// Initialization.
        /// </summary>
        /// <param name="owner"></param>
        public AsyncTimerExtension(MonoBehaviour owner) : base() { }

        //FIXME: AI code reviewer said, that Time.deltaTime is BAD in async.
        protected override async Task RunTimer(CancellationToken token)
        {
            try
            {
                CurrentTime = TimeSpan.Zero;
                var previousSecond = TimeSpan.Zero;

                while (!token.IsCancellationRequested)
                {
                    await Task.Yield();
                    CurrentTime = CurrentTime.Add(TimeSpan.FromSeconds(Time.deltaTime));

                    if ((int)CurrentTime.TotalSeconds > (int)previousSecond.TotalSeconds)
                    {
                        previousSecond = CurrentTime;
                        HandleSecondPassed();
                    }
                }
            }
            catch (OperationCanceledException) { }
        }

        //FIXME: AI code reviewer said, that Time.deltaTime is BAD in async.
        protected override async Task RunCountdown(CancellationToken token)
        {
            try
            {
                var previousSecond = CurrentTime;

                while (CurrentTime > TimeSpan.Zero && !token.IsCancellationRequested)
                {
                    await Task.Yield();
                    CurrentTime = CurrentTime.Subtract(TimeSpan.FromSeconds(Time.deltaTime));

                    if ((int)CurrentTime.TotalSeconds < (int)previousSecond.TotalSeconds)
                    {
                        previousSecond = CurrentTime;
                        HandleSecondPassed();
                    }
                }

                if (CurrentTime <= TimeSpan.Zero)
                {
                    InvokeInUnityContext(() =>
                    {
                        Complete();
                    });
                }
            }
            catch (OperationCanceledException) { }
        }
    }
}