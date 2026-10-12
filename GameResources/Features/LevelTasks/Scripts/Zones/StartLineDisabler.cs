namespace WinterDrift.Features.SaveSystem.Levels
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using DekDev.Extensions.Utils;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;
    using WinterDrift.Features.Triggers;
    
    /// <summary>
    /// Disable start line before cross one checkpoint.
    /// </summary>
    public sealed class StartLineDisabler : PlayerFacadeProvider
    {
        [SerializeField] private GameObject _lineObject = default;
        [SerializeField] private List<GameObject> directionCheckpoints = new List<GameObject>();
        private LapLineController _lapController = default;
        private CheckpointController _checkpointController = default;

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            _lapController = facade.Controllers.OfType<LapLineController>().FirstOrDefault();
            _checkpointController = facade.Controllers.OfType<CheckpointController>().FirstOrDefault();

            Subscribe();
        }
        protected override void OnInstanceDisposed()
        {
            Unsubscribe();

            base.OnInstanceDisposed();
        }

        protected override void OnDisable()
        {
            Unsubscribe();

            base.OnDisable();
        }

        private void OnLineCrossed()
        {
            SwitchObjects(false);
        }

        private void OnCheckpointCrossed()
        {
            SwitchObjects(true);
        }

        private void SwitchObjects(bool status)
        {
            if (_lineObject != null)
            {
                _lineObject.SetActive(status);

                directionCheckpoints.SetStatus(!status);
            }
        }

        private void Subscribe()
        {
            if (_lapController != null)
            {
                _lapController.onLineCrossed += OnLineCrossed;
            }

            if (_checkpointController != null)
            {
                _checkpointController.onCheckpointExit += OnCheckpointCrossed;
            }
        }
        private void Unsubscribe()
        {
            if (_lapController != null)
            {
                _lapController.onLineCrossed -= OnLineCrossed;
            }

            if (_checkpointController != null)
            {
                _checkpointController.onCheckpointExit -= OnCheckpointCrossed;
            }
        }
    }
}