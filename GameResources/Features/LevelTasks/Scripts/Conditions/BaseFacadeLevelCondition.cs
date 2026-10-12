namespace WinterDrift.Features.LevelTasks
{
    using DekDev.CDebug;
    using UnityEngine;
    using WinterDrift.Features.PlayerFacade;

    /// <summary>
    /// Base level condition connected with player facade.
    /// </summary>
    public class BaseFacadeLevelCondition : AbstractLevelCondition
    {
        [SerializeField] protected PlayerFacadeContainer container = default;
        protected PlayerFacade facade = default;

        public override void Initialize(LevelController controller)
        {
            base.Initialize(controller);

            if (isInProgress)
            {
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
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Unsubscribe();
        }

        /// <summary>
        /// Override in children for use facade.
        /// </summary>
        protected virtual void OnFacadeInited()
        {
            Unsubscribe();

            facade = container.Facade;
        }

        public override void UpdateConditionMet() { }

        protected override void UpdateBestScore() { }

        private void Unsubscribe()
        {
            if (container != null)
            {
                container.onInstanceInited -= OnFacadeInited;
            }
        }
    }
}