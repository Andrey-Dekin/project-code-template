namespace WinterDrift.Features.PlayerFacade
{
    using System.Collections.Generic;
    using DekDev.Extensions.Facade;
    using UnityEngine;

    /// <summary>
    /// Player facade class.
    /// </summary>
    public class PlayerFacade : AbstractFacade
    {
        /// <summary>
        /// Facade controllers.
        /// </summary>
        public IReadOnlyList<AbstractControllerFacade<PlayerFacade>> Controllers => controllers;

        /// <summary>
        /// Facade models.
        /// </summary>
        public IReadOnlyList<AbstractModelFacade<PlayerFacade>> Models => models;

        [SerializeField] protected List<AbstractControllerFacade<PlayerFacade>> controllers = new();
        [SerializeField] protected List<AbstractModelFacade<PlayerFacade>> models = new();

        protected virtual void Start()
        {
            models.ForEach(x => x.Initialize(this));
            controllers.ForEach(x=>x.Initialize(this));
        }
        protected virtual void OnDestroy()
        {
            controllers.ForEach(x => x.Dispose());
            models.ForEach(x => x.Dispose());
        }
    }
}
