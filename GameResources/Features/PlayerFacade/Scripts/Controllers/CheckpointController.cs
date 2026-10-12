namespace WinterDrift.Features.PlayerFacade
{
    using System;
    using DekDev.CDebug;
    using DekDev.Extensions.Facade;
    using UnityEngine;
    using WinterDrift.Features.Triggers;

    /// <summary>
    /// Checkpoint controller for prohibiting reverse movement.
    /// </summary>
    public class CheckpointController : AbstractControllerFacade<PlayerFacade>
    {
        /// <summary>
        /// Car entered checkpoint.
        /// </summary>
        public event Action onCheckpointEntered = delegate { };

        /// <summary>
        /// Car crossed checkpoint.
        /// </summary>
        public event Action onCheckpointExit = delegate { };

        /// <summary>
        /// Car drive back.
        /// </summary>
        public event Action onMovedBackward = delegate { };

        [SerializeField] protected CheckpointTrigger checkpointTrigger = default;
        protected Checkpoint currentCheckpoint = default;
        protected Checkpoint previousCheckpoint = default;

        public override void InitModule()
        {
            if (checkpointTrigger != null)
            {
                checkpointTrigger.onTriggerEntered += OnCheckpointEntered;
                checkpointTrigger.onTriggerExit += OnCheckpointExit;
            }
        }

        public override void Dispose()
        {
            DisposeData();

            base.Dispose();
        }

        protected void OnCheckpointEntered()
        {
            if (checkpointTrigger == null || currentCheckpoint == checkpointTrigger.CurrentZone) return;

            currentCheckpoint = checkpointTrigger.CurrentZone;

            onCheckpointEntered();

            if (previousCheckpoint == currentCheckpoint)
            {
                onMovedBackward();
            }
        }

        protected void OnCheckpointExit()
        {
            if (currentCheckpoint != null)
            {
                previousCheckpoint = currentCheckpoint;

                currentCheckpoint = null;

                onCheckpointExit();
            }
        }

        protected virtual void OnDisable() => DisposeData();

        protected virtual void DisposeData()
        {
            if (checkpointTrigger != null)
            {
                checkpointTrigger.onTriggerEntered -= OnCheckpointEntered;
                checkpointTrigger.onTriggerExit -= OnCheckpointExit;
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate()
        {
            if (checkpointTrigger == null)
            {
                CDebug.LogError("Checkpoint trigger field is null!");
            }
        }
#endif
    }
}