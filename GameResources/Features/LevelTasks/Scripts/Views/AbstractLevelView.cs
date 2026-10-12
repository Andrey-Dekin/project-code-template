namespace WinterDrift.Features.LevelTasks.Views
{
    using DekDev.CDebug;
    using UnityEngine;

    /// <summary>
    /// Abstract base class for view components that depend on LevelController.
    /// Handles initialization checking and automatic subscription management.
    /// </summary>
    public abstract class AbstractLevelView : MonoBehaviour
    {
        [SerializeField] protected LevelControllerContainer container = default;
        protected LevelController levelController => container.Component;
        protected bool isSubscribed = false;

        protected virtual void OnEnable()
        {
            if (container == null)
            {
                CDebug.LogError("Level container in View is empty!");
            }

            if (levelController != null)
            {
                OnLevelContainerInited();
            }
            else
            {
                container.onComponentChanged += OnLevelContainerInited;
            }
        }

        protected virtual void OnDisable()
        {
            UnsubscribeFromController();

            if (levelController != null)
            {
                levelController.onLevelInited -= HandleControllerInitialized;
            }
        }

        protected virtual void OnLevelContainerInited()
        {
            container.onComponentChanged -= OnLevelContainerInited;

            if (levelController.IsInitialized)
            {
                SubscribeToController();
                OnLevelInited();
                UpdateView();
            }
            else
            {
                levelController.onLevelInited += HandleControllerInitialized;
            }
        }

        protected virtual void HandleControllerInitialized()
        {
            if (levelController != null && levelController.IsInitialized)
            {
                SubscribeToController();
                OnLevelInited();
                UpdateView();

                levelController.onLevelInited -= HandleControllerInitialized;
            }
        }

        protected virtual void SubscribeToController()
        {
            if (levelController == null || isSubscribed) return;

            levelController.onWin += OnWin;
            levelController.onLose += OnLose;

            isSubscribed = true;
        }

        protected virtual void UnsubscribeFromController()
        {
            container.onComponentChanged -= OnLevelContainerInited;

            if (levelController == null || !isSubscribed) return;

            levelController.onWin -= OnWin;
            levelController.onLose -= OnLose;

            isSubscribed = false;
        }

        /// <summary>
        /// Actions on level inited.
        /// </summary>
        protected virtual void OnLevelInited() { }

        /// <summary>
        /// Actions on win level.
        /// </summary>
        protected virtual void OnWin() { }

        /// <summary>
        /// Actions on lose level.
        /// </summary>
        protected virtual void OnLose() { }

        /// <summary>
        /// Update view actions on enable and level controller initialization.
        /// </summary>
        protected abstract void UpdateView();
    }
}