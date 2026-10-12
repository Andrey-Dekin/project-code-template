namespace WinterDrift.Features.PlayerFacade.Views
{
    using System.Linq;

    /// <summary>
    /// Current speed view.
    /// </summary>
    public class SpeedView : AbstractPlayerProviderTextView
    {
        protected CarController carController = default;

        protected override void OnInstanceInited()
        {
            base.OnInstanceInited();

            carController = facade.Controllers.OfType<CarController>().FirstOrDefault();

            if (carController != null)
            {
                carController.onSpeedChanged += UpdateTextView;
            }
        }

        protected override void UpdateTextView() => textView.text = ((int)carController.CurrentSpeed).ToString();

        protected override void Unsubscribe()
        {
            if (carController != null)
            {
                carController.onSpeedChanged -= UpdateTextView;
            }
        }
    }
}
