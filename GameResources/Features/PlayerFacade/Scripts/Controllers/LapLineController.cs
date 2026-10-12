namespace WinterDrift.Features.PlayerFacade
{
    using System;
    using DekDev.CDebug;
    using DekDev.Extensions.Facade;
    using UnityEngine;
    using WinterDrift.Features.Triggers;

    /// <summary>
    /// Controller start/finish line.
    /// </summary>
    public class LapLineController : AbstractControllerFacade<PlayerFacade>
    {
        /// <summary>
        /// On the start/finish line has been crossed.
        /// </summary>
        public event Action onLineCrossed = delegate { };

        /// <summary>
        /// Current laps count.
        /// </summary>
        public int LapsCount => lapsCount;

        [SerializeField] protected StartFinishLineTrigger lineTrigger = default;
        protected int lapsCount = 0;

        public override void InitModule()
        {
            if (lineTrigger != null)
            {
                lineTrigger.onTriggerExit += OnLineCrossed;
            }
        }

        public override void Dispose()
        {
            DisposeData();

            base.Dispose();
        }

        protected void OnLineCrossed()
        {
            lapsCount++;

            onLineCrossed();
        }

        protected virtual void OnDisable() => DisposeData();

        protected virtual void DisposeData()
        {
            if (lineTrigger != null)
            {
                lineTrigger.onTriggerExit -= OnLineCrossed;
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (lineTrigger == null)
            {
                CDebug.LogError("Start/Finish trigger field is null!");
            }
        }
#endif
    }
}