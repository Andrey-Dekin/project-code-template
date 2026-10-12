namespace WinterDrift.Features.PlayerFacade
{
    using System;
    using DekDev.CDebug;
    using DekDev.Extensions.Facade;
    using UnityEngine;
    using WinterDrift.Features.Triggers;

    /// <summary>
    /// controller for player entry into a restricted area.
    /// </summary>
    public class LoseLineController : AbstractControllerFacade<PlayerFacade>
    {
        /// <summary>
        /// On the restricted line has been crossed.
        /// </summary>
        public event Action onLineEntered = delegate { };

        [SerializeField] protected LoseCheckpointTrigger checkpointTrigger = default;

        public override void InitModule()
        {
            if (checkpointTrigger != null)
            {
                checkpointTrigger.onTriggerEntered += OnLineEntered;
            }
        }

        public override void Dispose()
        {
            DisposeData();

            base.Dispose();
        }

        protected void OnLineEntered() => onLineEntered();

        protected virtual void OnDisable() => DisposeData();

        protected virtual void DisposeData()
        {
            if (checkpointTrigger != null)
            {
                checkpointTrigger.onTriggerExit -= OnLineEntered;
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (checkpointTrigger == null)
            {
                CDebug.LogError("Serialize field is null!");
            }
        }
#endif
    }
}