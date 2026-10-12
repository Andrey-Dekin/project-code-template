namespace WinterDrift.Features.LevelTasks
{
    using System.Linq;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;

    /// <summary>
    /// Disable gameObject after cross checkpoint.
    /// </summary>
    public class CheckpointCrossDisabler : PlayerFacadeProvider
    {
        protected CheckpointController controller = default;

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();
            controller = facade.Controllers.OfType<CheckpointController>().FirstOrDefault();

            Subscribe();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            Subscribe();
        }

        protected override void OnDisable()
        {
            Unsubscribe();

            base.OnDisable();
        }

        protected override void OnInstanceDisposed()
        {
            Unsubscribe();

            base.OnInstanceDisposed();
        }

        protected virtual void DisableObject() => gameObject.SetActive(false);

        protected virtual void Unsubscribe()
        {
            if (controller != null)
            {
                controller.onCheckpointExit -= DisableObject;
            }
        }

        protected virtual void Subscribe()
        {
            if (controller != null)
            {
                controller.onCheckpointExit += DisableObject;
            }
        }
    }
}