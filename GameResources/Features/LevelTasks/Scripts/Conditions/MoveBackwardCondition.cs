namespace WinterDrift.Features.LevelTasks
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using DekDev.CDebug;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;

    /// <summary>
    /// Condition for player don't move backward.
    /// </summary>
    public class MoveBackwardCondition : AbstractLevelCondition
    {
        protected const bool IN_PROGRESS = true;

        [SerializeField] protected PlayerFacadeContainer container = default;
        protected CheckpointController controller = default;

        public override void Initialize(LevelController controller)
        {
            base.Initialize(controller);

            isInProgress = IN_PROGRESS;

            if (container == null)
            {
                CDebug.LogError("Container field is Null!");
            }

            if (container != null && container.Facade != null)
            {
                OnFacadeInited();
            }
            else if (container != null)
            {
                container.onInstanceInited += OnFacadeInited;
            }
        }

        protected void OnFacadeInited()
        {
            Unsubscribe();

            controller = container.Facade?.Controllers.OfType<CheckpointController>().FirstOrDefault();

            if (controller != null)
            {
                controller.onMovedBackward += OnMoveBackward;
            }

            if (controller == null)
            {
                CDebug.LogError("Couldn't found controller in Facade");
            }
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Unsubscribe();

            if (controller != null)
            {
                controller.onMovedBackward -= OnMoveBackward;
            }
        }

        protected void OnMoveBackward()
        {
            if (controller != null)
            {
                controller.onMovedBackward -= OnMoveBackward;
            }

            if (isInProgress)
            {
                OnConditionFail();

                levelController.Finish();
            }
        }

        protected override void SetDefaultConditionStatus() => isConditionMet = true;

        private void Unsubscribe()
        {
            if (container != null)
            {
                container.onInstanceInited -= OnFacadeInited;
            }
        }

        public override void UpdateConditionMet() { }

        protected override void UpdateBestScore() { }
    }
}