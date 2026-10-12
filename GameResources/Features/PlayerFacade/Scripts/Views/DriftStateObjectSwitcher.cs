namespace WinterDrift.Features.PlayerFacade.Views
{
    using System.Collections.Generic;
    using System.Linq;
    using DekDev.Extensions.Objects;
    using DekDev.Extensions.Utils;
    using UnityEngine;

    /// <summary>
    /// Current speed view.
    /// </summary>
    public class DriftStateObjectSwitcher : PlayerFacadeProvider
    {
        [SerializeField] protected List<GameObject> enableObjects = new List<GameObject>();
        [SerializeField] protected List<GameObject> disableObjects = new List<GameObject>();
        protected CarController carController = default;

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            carController = (facade.Controllers).FirstOrDefault(x => x is CarController) as CarController;

            if (carController != null)
            {
                carController.onDriftStateChanged += UpdateView;
            }

            SwitchObjects(false);
        }

        protected override void OnInstanceDisposed()
        {
            base.OnInstanceDisposed();

            Unsubscribe();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            Unsubscribe();
        }

        protected virtual void SwitchObjects(bool status)
        {
            enableObjects.SetStatus(status);
            disableObjects.SetStatus(!status);
        }

        protected virtual void UpdateView() => SwitchObjects(carController.IsDrifting);

        protected virtual void Unsubscribe()
        {
            if (carController != null)
            {
                carController.onDriftStateChanged -= UpdateView;
            }
        }
    }
}
